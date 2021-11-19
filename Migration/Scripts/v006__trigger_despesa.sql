
-------------------------------------------------------------------------------------------------
--FUNCOES DE ATUALIZAR DESPESAS
create function atualizar_valor_despesa_update_despesa() returns trigger
    language plpgsql
as
$$
declare
    v_new numeric;
    v_old numeric;
    v_valor numeric;
    v_valor_old numeric;
    v_carteira carteira;
begin
    if (tg_op = 'INSERT') then
        select * into v_carteira from carteira where id = new.carteira_id;
        v_new = v_carteira.despesa + new.valor;
        update carteira set despesa = v_new, alterado_em = now() where id = v_carteira.id;
        return new;
    end if;
    if (tg_op = 'UPDATE') then
        -- Tratamento para que não seja alterado na carteira caso tente excluir mais de uma vez alguma despesa
        if (old.ativo = 0 AND new.ativo = 0) then
            return old;
        end if;
        --tipo
        --  2 = PARCELADA
        --  1 = CONTINUA
        --  0 = AVULSA
        --statusdespesa
        --  0 = CANCELADO
        --  1 = ABERTO
        --  2 = FINALIZADO
        --  3 = FIXADO
        --status
        --  0 = CANCELADO
        --  1 = ABERTO
        --  2 = FINALIZADO
--        if (new.tipo_id = 0 OR new.tipo_id = 1) then
            if (old.situacao_despesa_id = 0 AND new.situacao_despesa_id <> 0) then
                select * into v_carteira from carteira where id = new.carteira_id;
                v_new = v_carteira.despesa + new.valor;
                update carteira set despesa = v_new, alterado_em = now() where id = new.carteira_id;

                if (new.despesasfixas_id is not null) then
                    update despesasfixas
                    set status_id = 1,
                        ativo = new.ativo,
                        alterado_em = now()
                    where id = new.despesasfixas_id;
                end if;
            elseif (new.situacao_despesa_id = 0 OR new.ativo = 0) then
                select * into v_carteira from carteira where id = new.carteira_id;
                v_new = v_carteira.despesa - new.valor;
                update carteira set despesa = v_new, alterado_em = now() where id = new.carteira_id;

                if (new.despesasfixas_id is not null) then
                    update despesasfixas
                    set status_id = 0,
                        ativo = new.ativo,
                        alterado_em = now()
                    where id = new.despesasfixas_id;
                end if;
            else
                if (new.valor <> old.valor) then
                    select * into v_carteira from carteira where id = new.carteira_id;
                    v_old = v_carteira.despesa - old.valor;
                    v_new = v_old + new.valor;
                    update carteira set despesa = v_new, alterado_em = now() where id = new.carteira_id;
                end if;
            end if;
--        end if;
        return new;
    end if;
end;
$$;

--alter function atualizar_valor_despesa_update_despesa() owner to postgres;

create trigger atualizar_valor_despesa_update_despesa
    after INSERT OR UPDATE on despesa
    for each row execute function atualizar_valor_despesa_update_despesa()
    
-------------------------------------------------------------------------------------------------