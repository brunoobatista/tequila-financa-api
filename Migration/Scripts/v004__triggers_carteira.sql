﻿
-------------------------------------------------------------------------------------------------
-- TRIGGER DE ATUALIZAR DESPESAS FIXAS QUANTO INSERE CARTEIRA
create or replace function inserir_despesa_fixa_pela_carteira() returns trigger
    language plpgsql
as
$$
declare
    v_parcela_atual int;
    v_despesa numeric;
    reg record;
begin
        --tipo
        --  0 = AVULSA
        --  1 = CONTINUA
        --  2 = PARCELADA
        --situacaodespesa
        --  0 = CANCELADA
        --  1 = ABERTA
        --  2 = FINALIZADA
        --  3 = FIXADA
        --status
        --  0 = CANCELADO
        --  1 = ABERTO
        --  2 = FINALIZADO
    if (tg_op = 'INSERT') then
        --         create temp table dftmp as
        --           select * from despesasfixas df where df.usuario_id = new.usuario_id and ativo = 1 and status_id = 1;
        v_despesa = 0;
        for reg in (select * from despesasfixas df where df.usuario_id = new.usuario_id and ativo = 1 and status_id = 1) loop
            if (reg.total_parcelas IS NOT NULL) then
                if (reg.parcela_atual < reg.total_parcelas) then
                    v_parcela_atual = reg.parcela_atual + 1;
                    insert into despesa(
                        usuario_id, carteira_id, despesasfixas_id, descricao, valor, total_parcelas, parcela_atual, data_vencimento, tipo_id, situacao_despesa_id
                    ) values (
                                 new.usuario_id, new.id, reg.id, reg.descricao, reg.valor, reg.total_parcelas, v_parcela_atual, reg.data_vencimento, 2, 3
                             );
                    update despesasfixas set parcela_atual = v_parcela_atual, alterado_em = now() where id = reg.id;
                else
                    update despesasfixas set status_id = 2, alterado_em = now() where id = reg.id;
                    continue;
                end if;
            else
                insert into despesa(
                    usuario_id, carteira_id, despesasfixas_id, descricao, valor, data_vencimento, tipo_id, situacao_despesa_id
                ) values (
                             new.usuario_id, new.id, reg.id, reg.descricao, reg.valor, reg.data_vencimento, 1, 1
                         );
            end if;
            v_despesa = v_despesa + reg.valor;
        end loop;
  
        update carteira set despesa = v_despesa, alterado_em = now() where id = new.id;

        return new;
    end if;
    return null;
end;
$$;

--alter function inserir_despesa_fixa_pela_carteira() owner to postgres;


create trigger inserir_despesa_fixa_pela_carteira
    after INSERT on carteira
    for each row execute function inserir_despesa_fixa_pela_carteira()

-------------------------------------------------------------------------------------------------


-------------------------------------------------------------------------------------------------
-- FUNCAO REMOVER PERMISSAO DE REATIVAR
create or replace function remover_permissao_reativar_carteira() returns trigger
    language plpgsql
as
$$
declare
    reg record;
begin
    if (tg_op = 'INSERT') then
        for reg in (select * from carteira c where c.usuario_id = new.usuario_id and c.ativo = 1 and c.can_reativar = true) loop
            update carteira set can_reativar = false, alterado_em = now() where id = reg.id;
        end loop;
        return new;
    end if;
end;
$$;

--alter function remover_permissao_reativar_carteira() owner to postgres;

create trigger remover_permissao_reativar_carteira
    before INSERT on carteira
    for each row execute function remover_permissao_reativar_carteira()
-------------------------------------------------------------------------------------------