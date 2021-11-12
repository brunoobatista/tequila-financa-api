create or replace function atualizar_renda_extra() returns trigger
    language plpgsql
as
$$
declare
    v_carteira_id bigint;
    v_new numeric;
    v_old numeric;
    v_carteira carteira;
begin
    select * into v_carteira from carteira where id = new.carteira_id;
    if (tg_op = 'INSERT') then
        v_carteira_id = v_carteira.id;
        v_new = v_carteira.renda_extra + new.valor;
        update carteira set renda_extra = v_new, alterado_em = now()where id = new.carteira_id;
    end if;
    if (tg_op = 'UPDATE') then
        if (new.ativo = 1) then
            if (new.valor <> old.valor) then
                v_old = v_carteira.renda_extra - old.valor;
                v_new = v_old + new.valor;
                update carteira set renda_extra = v_new, alterado_em = now() where id = new.carteira_id;
            end if;
        end if;
        if (new.ativo = 0) then
            v_old = v_carteira.renda_extra - old.valor;
            update carteira set renda_extra = v_old, alterado_em = now() where id = new.carteira_id;
        end if;
    end if;
    return new;
end;
$$;

--drop trigger atualizar_renda_extra on rendaadicional;
create trigger atualizar_renda_extra
    after INSERT OR UPDATE on rendaadicional
    for each row execute function atualizar_renda_extra()

