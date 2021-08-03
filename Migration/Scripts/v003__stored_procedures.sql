create or replace procedure inserirdespesasfixas(INOUT usuario_id bigint, INOUT carteira_id bigint, INOUT descri character varying, INOUT valor_prev numeric, INOUT data_venc timestamp without time zone, INOUT tipo_id integer, INOUT total_parc integer, INOUT id_despesa bigint)
    language plpgsql
as
$$
DECLARE
    v_id bigint;
    v_carteira_valor numeric;
    novo_valor numeric;
BEGIN

    CASE tipo_id
        when 1 then
            --              Primeiro será inseiro a despesas fixas
            INSERT INTO despesasfixas(usuario_id, descricao, valor_previsto, data_vencimento, tipo_id)
            VALUES (usuario_id, descri, valor_prev, data_venc, tipo_id)
            RETURNING id INTO v_id;

            id_despesa = v_id;
            --             Depois sera criado a despesa fixa em si, e entao calcular o valor de despesa da carteira
--             Esse valor deve ser temporario, o valor previsto sera retirado da carteira
--             e depois adicionado o valor da finalização
            INSERT INTO despesa(usuario_id, carteira_id, despesasfixas_id, descricao, valor_previsto, data_vencimento, tipo_id)
            VALUES (usuario_id, carteira_id, v_id, descri, valor_prev, data_venc, tipo_id);

        when 2 then
            INSERT INTO despesasfixas(usuario_id, descricao, valor_previsto, parcela_atual, total_parcelas, data_vencimento, tipo_id)
            values(usuario_id, descri, valor_prev, 1, total_parc, data_venc, tipo_id)
            RETURNING id INTO v_id;
            id_despesa = v_id;

            INSERT INTO despesa(usuario_id, carteira_id, despesasfixas_id, descricao, valor, data_vencimento, parcela_atual, total_parcelas, tipo_id)
            VALUES (usuario_id, carteira_id, v_id, descri, valor_prev, data_venc, 1, total_parc, tipo_id);

        end case;

    select c.despesa into v_carteira_valor from carteira as c where c.id = carteira_id;
    IF v_carteira_valor is null then
        v_carteira_valor = 0;
    end if;
    novo_valor = v_carteira_valor + valor_prev;
    update carteira set despesa = novo_valor where id = carteira_id;


EXCEPTION WHEN OTHERS THEN
    RAISE exception '% %', SQLERRM, SQLSTATE;
END
$$;

alter procedure inserirdespesasfixas(inout bigint, inout bigint, inout varchar, inout numeric, inout timestamp, inout integer, inout integer, inout bigint) owner to postgres;





create procedure finalizardespesacontinua(
                    INOUT id_despesa bigint,
                    INOUT valor_final numeric,
                    INOUT result integer
                )
    language plpgsql
as
$$
declare
    v_id_dps bigint;
    v_id_cart bigint;
    v_despesasfx despesasfixas;
    old_valor numeric;
    new_valor numeric;
    valor_prev numeric;
begin
    select d.despesasfixas_id, d.carteira_id, d.valor_previsto into v_id_dps, v_id_cart, valor_prev from despesa d where d.id = id_despesa;
    select fx.* into v_despesasfx from despesasfixas fx where fx.id = v_id_dps;

    if v_despesasfx.tipo_id = 1 then
        select c.despesa into old_valor from carteira as c where c.id = v_id_cart;
        old_valor = old_valor  - valor_prev;
        new_valor = old_valor + valor_final;
        update despesa set valor = valor_final, alterado_em = now(), status_id = 2 where id = id_despesa;
        update carteira set despesa = new_valor, alterado_em = now() where id = v_id_cart;
        result = 1;
    end if;

EXCEPTION WHEN OTHERS THEN
    RAISE exception '% %', SQLERRM, SQLSTATE;
end
$$;

alter procedure finalizardespesacontinua(inout bigint, inout numeric, inout integer) owner to postgres;



