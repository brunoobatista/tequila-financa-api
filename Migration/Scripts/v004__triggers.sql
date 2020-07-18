create or replace function inserir_despesa_fixa_pela_carteira()
    returns trigger as $body$
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
                    raise notice 'tem parcela';
                    if reg.parcela_atual < reg.total_parcelas then
                        v_parcela_atual = reg.parcela_atual + 1;
                        insert into despesafixa(
                            carteira_id, despesasfixas_id, descricao, valor, total_parcelas, parcela_atual, data_vencimento, tipo_id
                        ) values (
                                     new.id, reg.id, reg.descricao, reg.valor_previsto, reg.total_parcelas, v_parcela_atual, reg.data_vencimento, reg.tipo_id
                                 );
                        update despesasfixas set parcela_atual = v_parcela_atual, alterado_em = now() where id = reg.id;
                    else
                        update despesasfixas set status_id = 2, alterado_em = now() where id = reg.id;
                    end if;
                else
                    raise notice 'nao tem';
                    insert into despesafixa(
                        carteira_id, despesasfixas_id, descricao, valor_previsto, data_vencimento, tipo_id
                    ) values (
                                 new.id, reg.id, reg.descricao, reg.valor_previsto, reg.data_vencimento, reg.tipo_id
                             );
                end if;
                v_despesa = v_despesa + reg.valor_previsto;
            end loop;
        raise notice '%', v_despesa;
        update carteira set despesa = v_despesa where id = new.id;
        new.despesa = v_despesa;
        return new;
    end if;
    return null;
end;
$body$
    LANGUAGE plpgsql;

create trigger inserir_despesa_fixa_pela_carteira
    after INSERT on carteira
    for each row execute function inserir_despesa_fixa_pela_carteira()

