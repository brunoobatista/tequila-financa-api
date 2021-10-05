
-------------------------------------------------------------------------------------------------
--FUNCOES DE ATUALIZAR DESPESAS
create or replace function atualizar_valor_despesa_update_despesa() returns trigger
    language plpgsql
as
$$
declare
    v_new numeric;
    v_old numeric;
    v_valor numeric;
    v_carteira carteira;
begin
    if (tg_op = 'INSERT') then
    
        if (new.tipo_id = 0 OR new.tipo_id = 2) then
            v_valor = new.valor;
        ELSE
            v_valor = new.valor_previsto; 
        end if;
            
        select * into v_carteira from carteira where id = new.carteira_id;
        v_new = v_carteira.despesa + v_valor;
        update carteira set despesa = v_new, alterado_em = now() where id = v_carteira.id;
        return new;
    end if;
    if (tg_op = 'UPDATE') then
        --tipo 2 = parcelada
        --tipo 0 = AVULSA
        if (new.tipo_id = 0 OR new.tipo_id = 2) then
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
$$;

--alter function atualizar_valor_despesa_update_despesa() owner to postgres;

create trigger atualizar_valor_despesa_update_despesa
    after INSERT OR UPDATE on despesa
    for each row execute function atualizar_valor_despesa_update_despesa()
    
-------------------------------------------------------------------------------------------------