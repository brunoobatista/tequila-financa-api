-- auto-generated definition
create type despesasfixastype as
(
    id              bigint,
    usuario_id      bigint,
    carteira_id     bigint,
    descricao       varchar(255),
    valor_previsto  numeric(15, 2),
    data_vencimento timestamp(0),
    total_parcelas  integer,
    tipo_id         integer
);