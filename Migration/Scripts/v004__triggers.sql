create or replace function inserir_despesa_fixa_pela_carteira() returns trigger
    language plpgsql
as
$$
declare
    v_parcela_atual int;
    v_despesa numeric;
    reg record;
begin
    if (tg_op = 'INSERT') then
        --         create temp table dftmp as
--                 select * from despesasfixas df where df.usuario_id = new.usuario_id and ativo = 1 and status_id = 1;
        v_despesa = 0;
        for reg in (select * from despesasfixas df where df.usuario_id = new.usuario_id and ativo = 1 and status_id = 1) loop
                if reg.total_parcelas is not null then
                    if reg.parcela_atual < reg.total_parcelas then
                        v_parcela_atual = reg.parcela_atual + 1;
                        insert into despesafixa(
                            carteira_id, despesasfixas_id, descricao, valor, total_parcelas, parcela_atual, data_vencimento, tipo_id, status_id
                        ) values (
                                     new.id, reg.id, reg.descricao, reg.valor_previsto, reg.total_parcelas, v_parcela_atual, reg.data_vencimento, reg.tipo_id, 2
                                 );
                        update despesasfixas set parcela_atual = v_parcela_atual, alterado_em = now() where id = reg.id;
                    else
                        update despesasfixas set status_id = 2, alterado_em = now() where id = reg.id;
                    end if;
                else
                    insert into despesafixa(
                        carteira_id, despesasfixas_id, descricao, valor_previsto, data_vencimento, tipo_id
                    ) values (
                                 new.id, reg.id, reg.descricao, reg.valor_previsto, reg.data_vencimento, reg.tipo_id
                             );
                end if;
                v_despesa = v_despesa + reg.valor_previsto;
            end loop;
        update carteira set despesa = v_despesa where id = new.id;
        new.despesa = v_despesa;
        return new;
    end if;
    return null;
end;
$$;

alter function inserir_despesa_fixa_pela_carteira() owner to postgres;



create trigger inserir_despesa_fixa_pela_carteira
    after INSERT on carteira
    for each row execute function inserir_despesa_fixa_pela_carteira()



create or replace function atualizar_valor_despesa_update_despesafixa()
    returns trigger as $body$
declare
    v_carteira_id bigint;
    v_new numeric;
    v_old numeric;
    v_carteira carteira;
begin
    if (tg_op = 'UPDATE') then
        --tipo 2 = parcelado
        if (new.tipo_id = 2) then
            if (new.valor <> old.valor) then
                select * into v_carteira from carteira where id = new.carteira_id;
                v_old = v_carteira.despesa - old.valor;
                v_new = v_old + new.valor;
                update carteira set despesa = v_new, alterado_em = now() where id = new.carteira_id;
            end if;
        end if;
        return new;
    end if;
end;
$body$
    language plpgsql;

create trigger atualizar_valor_despesa_update_despesafixa
    after UPDATE on despesafixa
    for each row execute function atualizar_valor_despesa_update_despesafixa()