create or replace procedure inserirdespesasfixas(
    INOUT usuario_id bigint,
    INOUT carteira_id bigint,
    INOUT descricao varchar,
    INOUT valor_previsto numeric,
    INOUT data_vencimento timestamp,
    INOUT tipo_id timestamp,
    INOUT id bigint
)
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
            VALUES (usuario_id, descricao, valor_previsto, data_vencimento, tipo_id)
            RETURNING id INTO v_id;

            id = v_id;
            --             Depois sera criado a despesa fixa em si, e entao calcular o valor de despesa da carteira
--             Esse valor deve ser temporario, o valor previsto sera retirado da carteira
--             e depois adicionado o valor da finalização
            INSERT INTO despesafixa(carteira_id, despesasfixas_id, descricao, valor_previsto, data_vencimento)
            VALUES (carteira_id, v_id, descricao, valor_previsto, data_vencimento);

        when 2 then
            INSERT INTO despesasfixas(usuario_id, descricao, valor_previsto, parcela_atual, total_parcelas, data_vencimento, tipo_id)
            values(usuario_id, descricao, valor_previsto, 1, total_parcelas, data_vencimento, tipo_id)
            RETURNING id INTO v_id;
            id = v_id;

            INSERT INTO despesafixa(carteira_id, despesasfixas_id, descricao, valor, data_vencimento, parcela_atual, total_parcelas)
            VALUES (carteira_id, v_id, descricao, valor_previsto, data_vencimento, 1, total_parcelas);

        end case;

    select c.despesa into v_carteira_valor from carteira as c where c.id = carteira_id;
    IF v_carteira_valor is null then
        v_carteira_valor = 0;
    end if;
    novo_valor = v_carteira_valor + valor_previsto;
    update carteira set despesa = novo_valor where id = carteira_id;
    COMMIT;

EXCEPTION WHEN OTHERS THEN
    RAISE exception '% %', SQLERRM, SQLSTATE;
    ROLLBACK;
END
$$;




create procedure inserirdespesasfixas(INOUT dp despesasfixastype)
    language plpgsql
as
$$
DECLARE
    v_id bigint;
    v_carteira_valor numeric;
    novo_valor numeric;
BEGIN
    CASE dp.tipo_id
        when 1 then
            --              Primeiro será inseiro a despesas fixas
            INSERT INTO despesasfixas(usuario_id, descricao, valor_previsto, data_vencimento, tipo_id)
            VALUES (dp.usuario_id, dp.descricao, dp.valor_previsto, dp.data_vencimento, dp.tipo_id)
            RETURNING id INTO v_id;

            dp.id = v_id;
            --             Depois sera criado a despesa fixa em si, e entao calcular o valor de despesa da carteira
--             Esse valor deve ser temporario, o valor previsto sera retirado da carteira
--             e depois adicionado o valor da finalização
            INSERT INTO despesafixa(carteira_id, despesasfixas_id, descricao, valor_previsto, data_vencimento)
            VALUES (dp.carteira_id, v_id, dp.descricao, dp.valor_previsto, dp.data_vencimento);

        when 2 then
            INSERT INTO despesasfixas(usuario_id, descricao, valor_previsto, parcela_atual, total_parcelas, data_vencimento, tipo_id)
            values(dp.usuario_id, dp.descricao, dp.valor_previsto, 1, dp.total_parcelas, dp.data_vencimento, dp.tipo_id)
            RETURNING id INTO v_id;
            dp.id = v_id;

            INSERT INTO despesafixa(carteira_id, despesasfixas_id, descricao, valor, data_vencimento, parcela_atual, total_parcelas)
            VALUES (dp.carteira_id, v_id, dp.descricao, dp.valor_previsto, dp.data_vencimento, 1, dp.total_parcelas);

        end case;

    select c.despesa into v_carteira_valor from carteira as c where c.id = dp.carteira_id;
    IF v_carteira_valor is null then
        v_carteira_valor = 0;
    end if;
    novo_valor = v_carteira_valor + dp.valor_previsto;
    update carteira set despesa = novo_valor where id = dp.carteira_id;
    COMMIT;

EXCEPTION WHEN OTHERS THEN
    RAISE exception '% %', SQLERRM, SQLSTATE;
    ROLLBACK;
END
$$;


create procedure finalizardespesacontinua(IN id_despesa bigint, IN valor_final numeric)
    language plpgsql
as
$$
declare
    v_id_dps bigint;
    v_id_cart bigint;
    v_despesa despesasfixas;
    old_valor numeric;
    new_valor numeric;
    valor_prev numeric;
begin
    select d.despesasfixas_id, d.carteira_id, d.valor_previsto into v_id_dps, v_id_cart, valor_prev from despesafixa d where d.id = id_despesa;
    select fx.* into v_despesa from despesasfixas fx where fx.id = v_id_dps;

    if v_despesa.tipo_id = 1 then
        select c.despesa into old_valor from carteira as c where c.id = v_id_cart;
        old_valor = old_valor  - valor_prev;
        new_valor = old_valor + valor_final;
        update carteira set despesa = new_valor where id = v_id_cart;
        COMMIT;
    end if;

EXCEPTION WHEN OTHERS THEN
    RAISE exception '% %', SQLERRM, SQLSTATE;
    ROLLBACK;
end
$$;


alter procedure finalizardespesacontinua(bigint, numeric) owner to postgres;


