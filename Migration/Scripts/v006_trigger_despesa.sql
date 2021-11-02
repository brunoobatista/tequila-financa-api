
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
        if (new.valor is not null) then
            v_valor = new.valor;
        else
            v_valor = new.valor_previsto;
        end if;

        select * into v_carteira from carteira where id = new.carteira_id;
        v_new = v_carteira.despesa + v_valor;
        update carteira set despesa = v_new, alterado_em = now() where id = v_carteira.id;
        return new;
    end if;
    if (tg_op = 'UPDATE') then
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
        if (new.tipo_id = 0 OR new.tipo_id = 1) then
            if (old.status_id = 0 AND new.status_id <> old.status_id) then
                select * into v_carteira from carteira where id = new.carteira_id;
                if (new.valor is not null) then
                    v_valor = new.valor;
                else
                    v_valor = new.valor_previsto;
                end if;
                v_new = v_carteira.despesa + v_valor;
                update carteira set despesa = v_new, alterado_em = now() where id = new.carteira_id;

                if (new.despesasfixas_id is not null) then
                    update despesasfixas
                    set status_id = 1,
                        ativo = new.ativo,
                        alterado_em = now()
                    where id = new.despesasfixas_id;
                end if;
            elseif (new.status_id = 0 OR new.ativo = 0) then
                select * into v_carteira from carteira where id = new.carteira_id;
                if (new.valor is not null) then
                    v_valor = new.valor;
                else
                    v_valor = new.valor_previsto;
                end if;
                v_new = v_carteira.despesa - v_valor;
                update carteira set despesa = v_new, alterado_em = now() where id = new.carteira_id;

                if (new.despesasfixas_id is not null) then
                    update despesasfixas
                    set status_id = new.status_id,
                        ativo = new.ativo,
                        alterado_em = now()
                    where id = new.despesasfixas_id;
                end if;
            else
                if (new.valor <> old.valor) then
                    select * into v_carteira from carteira where id = new.carteira_id;
                    if (new.valor is not null) then
                        v_valor = new.valor;
                    else
                        v_valor = new.valor_previsto;
                    end if;
                    if (old.valor is not null) then
                        v_valor_old = old.valor;
                    else
                        v_valor_old = old.valor_previsto;
                    end if;
                    v_old = v_carteira.despesa - v_valor_old;
                    v_new = v_old + v_valor;
                    update carteira set despesa = v_new, alterado_em = now() where id = new.carteira_id;
                end if;
            end if;
        end if;
        return new;
    end if;
end;
$$;

--alter function atualizar_valor_despesa_update_despesa() owner to postgres;

create trigger atualizar_valor_despesa_update_despesa
    after INSERT OR UPDATE on despesa
    for each row execute function atualizar_valor_despesa_update_despesa()
    
-------------------------------------------------------------------------------------------------