-- MySQL Script generated by MySQL Workbench
-- seg 04 mai 2020 20:33:31 -03
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

/* SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0; */
/* SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0; */
/* SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES'; */

-- -----------------------------------------------------
-- Table public.Status
-- -----------------------------------------------------
DROP TABLE IF EXISTS public.Status CASCADE;

CREATE TABLE IF NOT EXISTS public.Status (
  id INT NOT NULL,
  nome VARCHAR(45) NOT NULL,
  PRIMARY KEY (id))
;

-- -----------------------------------------------------
-- Table public.Tipo
-- -----------------------------------------------------
DROP TABLE IF EXISTS public.Tipo CASCADE;

CREATE TABLE IF NOT EXISTS public.Tipo (
  id INT NOT NULL,
  nome VARCHAR(45) NOT NULL,
  PRIMARY KEY (id))
;

-- -----------------------------------------------------
-- Table public.Endereco
-- -----------------------------------------------------
DROP TABLE IF EXISTS public.Endereco CASCADE;

CREATE SEQUENCE IF NOT EXISTS public.Endereco_seq;

CREATE TABLE IF NOT EXISTS public.Endereco (
  id BIGINT NOT NULL DEFAULT NEXTVAL ('public.Endereco_seq'),
  rua VARCHAR(45) NOT NULL,
  cep VARCHAR(45) NOT NULL,
  numero VARCHAR(45) NOT NULL,
  complemento VARCHAR(45) NULL,
  PRIMARY KEY (id)
  )
;


-- -----------------------------------------------------
-- Table public.Usuario
-- -----------------------------------------------------
DROP TABLE IF EXISTS public.Usuario CASCADE;

CREATE SEQUENCE IF NOT EXISTS public.Usuario_seq;

CREATE TABLE IF NOT EXISTS public.Usuario (
  id BIGINT NOT NULL DEFAULT NEXTVAL ('public.Usuario_seq'),
  email VARCHAR(45) NOT NULL,
  senha VARCHAR(100) NOT NULL,
  nome VARCHAR(50) NOT NULL,
  avatar VARCHAR(255) NULL,
  cpf_cnpj VARCHAR(45) NULL,
  renda NUMERIC(15,2) NOT NULL,
  tipo_renda VARCHAR(255) NULL,
  criado_em TIMESTAMP(0) NOT NULL DEFAULT NOW(),
  alterado_em TIMESTAMP(0) NULL,
  ativo INTEGER NOT NULL DEFAULT 1,
  endereco_id BIGINT NULL UNIQUE,
  PRIMARY KEY (id),
  CONSTRAINT fk_usuario_endereco
    FOREIGN KEY (endereco_id)
    REFERENCES public.Endereco (id)
    ON DELETE CASCADE
    ON UPDATE NO ACTION
  )
;

CREATE UNIQUE INDEX IF NOT EXISTS email_UNIQUE ON public.Usuario (email ASC);


-- -----------------------------------------------------
-- Table public.Carteira
-- -----------------------------------------------------
DROP TABLE IF EXISTS public.Carteira CASCADE;

CREATE SEQUENCE IF NOT EXISTS public.Carteira_seq;

CREATE TABLE IF NOT EXISTS public.Carteira (
  id BIGINT NOT NULL DEFAULT NEXTVAL ('public.Carteira_seq'),
  usuario_id BIGINT NOT NULL,
  status_id INT NOT NULL DEFAULT 1,
  renda NUMERIC(15,2) NOT NULL,
  despesa NUMERIC(15,2) NULL,
  renda_extra NUMERIC(15,2) NULL,
  criado_em TIMESTAMP(0) NOT NULL DEFAULT NOW(),
  alterado_em TIMESTAMP(0) NULL,
  ativo INTEGER NOT NULL DEFAULT 1,
  PRIMARY KEY (id),
  CONSTRAINT fk_usuario_carteira_id
    FOREIGN KEY (usuario_id)
    REFERENCES public.Usuario (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_status_carteira_id
    FOREIGN KEY (status_id)
    REFERENCES public.Status (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
;

CREATE INDEX IF NOT EXISTS fk_usuario_carteira_idx ON public.Carteira (usuario_id ASC);

CREATE INDEX IF NOT EXISTS fk_status_carteira_idx ON public.Carteira (status_id ASC);

-- -----------------------------------------------------
-- Table public.RendaAdicional
-- -----------------------------------------------------
DROP TABLE IF EXISTS public.RendaAdicional CASCADE;

CREATE SEQUENCE IF NOT EXISTS public.RendaAdicional_seq;

CREATE TABLE IF NOT EXISTS public.RendaAdicional (
  id BIGINT NOT NULL DEFAULT NEXTVAL ('public.RendaAdicional_seq'),
  usuario_id BIGINT NOT NULL,
  carteira_id BIGINT NOT NULL,
  valor NUMERIC(15,2) NULL,
  descricao VARCHAR(255) NULL,
  criado_em TIMESTAMP(0) NOT NULL DEFAULT NOW(),
  alterado_em TIMESTAMP(0) NULL,
  ativo INTEGER NOT NULL DEFAULT 1,
  PRIMARY KEY (id),
  CONSTRAINT fk_usuario_rendaadd_id
    FOREIGN KEY (usuario_id)
    REFERENCES public.Usuario (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_carteira_rendaadd_id
    FOREIGN KEY (carteira_id)
    REFERENCES public.Carteira (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
;

CREATE INDEX IF NOT EXISTS fk_usuario_rendaadd_idx ON public.RendaAdicional (usuario_id ASC);

CREATE INDEX IF NOT EXISTS fk_carteira_rendaadd_idx ON public.RendaAdicional (carteira_id ASC);

-- -----------------------------------------------------
-- Table public.DespesasFixas
-- -----------------------------------------------------
DROP TABLE IF EXISTS public.DespesasFixas CASCADE;

CREATE SEQUENCE IF NOT EXISTS public.DespesasFixas_seq;

CREATE TABLE IF NOT EXISTS public.DespesasFixas (
  id BIGINT NOT NULL DEFAULT NEXTVAL ('public.DespesasFixas_seq'),
  usuario_id BIGINT NOT NULL,
  descricao VARCHAR(255) NOT NULL,
  valor_previsto NUMERIC(15,2) NULL,
  parcela_atual INT NULL,
  total_parcelas INT NULL,
  data_vencimento timestamp(0) NULL,
  criado_em TIMESTAMP(0) NOT NULL DEFAULT NOW(),
  alterado_em TIMESTAMP(0) NULL,
  ativo INTEGER NOT NULL DEFAULT 1,
  status_id INT NOT NULL DEFAULT 1,
  tipo_id INT NOT NULL,
  PRIMARY KEY (id),
  CONSTRAINT fk_usuario_despesafx_id
    FOREIGN KEY (usuario_id)
    REFERENCES public.Usuario (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_status_despesasfixas_id
    FOREIGN KEY (status_id)
    REFERENCES public.Status (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_tipo_despesasfixas_id
    FOREIGN KEY (tipo_id)
    REFERENCES public.Tipo (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);

CREATE INDEX IF NOT EXISTS fk_usuario_despesasfxs_idx ON public.DespesasFixas (usuario_id ASC);

-- -----------------------------------------------------
-- Table public.DespesaFixa
-- -----------------------------------------------------
DROP TABLE IF EXISTS public.DespesaFixa CASCADE;

CREATE SEQUENCE IF NOT EXISTS public.DespesaFixa_seq;

CREATE TABLE IF NOT EXISTS public.DespesaFixa (
  id BIGINT NOT NULL DEFAULT NEXTVAL ('public.DespesaFixa_seq'),
  carteira_id BIGINT NOT NULL,
  despesasfixas_id BIGINT NOT NULL,
  descricao VARCHAR(255) NOT NULL,
  valor_previsto NUMERIC(15,2) NULL,
  valor NUMERIC(15,2) NULL,
  total_parcelas INT NULL,
  parcela_atual INT NULL,
  data_vencimento timestamp(0) NULL,
  criado_em TIMESTAMP(0) NOT NULL DEFAULT NOW(),
  alterado_em TIMESTAMP(0) NULL,
  ativo INTEGER NOT NULL DEFAULT 1,
  tipo_id INT NOT NULL,
  status_id INT NOT NULL DEFAULT 1,
  PRIMARY KEY (id),
  CONSTRAINT fk_carteira_despesafx_id
    FOREIGN KEY (carteira_id)
    REFERENCES public.Carteira (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_despesasfixas_id_despesafx_id
    FOREIGN KEY (despesasfixas_id)
    REFERENCES public.DespesasFixas (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_tipo_carteira_id
    FOREIGN KEY (tipo_id)
    REFERENCES public.Tipo (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_status_despesafixa_id
    FOREIGN KEY (status_id)
    REFERENCES public.Status (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  UNIQUE(carteira_id,despesasfixas_id)
);

CREATE INDEX IF NOT EXISTS fk_carteira_despesafx_idx ON public.DespesaFixa (carteira_id ASC);

CREATE INDEX IF NOT EXISTS fk_listafixadespesa_despesafx_idx ON public.DespesaFixa (despesasfixas_id ASC);

-- -----------------------------------------------------
-- Table public.DespesaVariavel
-- -----------------------------------------------------
DROP TABLE IF EXISTS public.DespesaVariavel CASCADE;

CREATE SEQUENCE IF NOT EXISTS public.DespesaVariavel_seq;

CREATE TABLE IF NOT EXISTS public.DespesaVariavel (
  id BIGINT NOT NULL DEFAULT NEXTVAL ('public.DespesaVariavel_seq'),
  carteira_id BIGINT NOT NULL,
  valor NUMERIC(15,2) NOT NULL,
  descricao VARCHAR(255) NOT NULL,
  criado_em TIMESTAMP(0) NOT NULL DEFAULT NOW(),
  alterado_em TIMESTAMP(0) NULL,
  ativo INTEGER NOT NULL DEFAULT 1,
  status_id INT NOT NULL DEFAULT 1,
  PRIMARY KEY (id),
  CONSTRAINT fk_carteira_despesavrl_id
    FOREIGN KEY (carteira_id)
    REFERENCES public.Carteira (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_status_despesavariavel_id
    FOREIGN KEY (status_id)
    REFERENCES public.Status (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
    )
;

CREATE INDEX IF NOT EXISTS fk_carteira_despesavrl_idx ON public.DespesaVariavel (carteira_id ASC);


-- ************************************** "EnderecoCompra"
CREATE SEQUENCE IF NOT EXISTS public.EnderecoCompra_seq;
DROP TABLE IF EXISTS public.EnderecoCompra CASCADE;
CREATE TABLE EnderecoCompra
(
 id         bigint NOT NULL,
 usuario_id bigint NOT NULL,
 rua        varchar(50) NOT NULL,
 estado     varchar(50) NULL,
 cidade     varchar(50) NOT NULL,
 cep        varchar(20) NOT NULL,
 nome       varchar(100) NOT NULL,
 descricao  varchar(255) NULL,
 latitude   double precision NOT NULL,
 longitude  double precision NOT NULL,
 PRIMARY KEY (id),
 CONSTRAINT fk_usuario_endereco_compra_id FOREIGN KEY (usuario_id) REFERENCES public.Usuario (id)
);


-- -----------------------------------------------------
-- Data for table public.Endereco
-- -----------------------------------------------------
INSERT INTO public.Endereco (id, rua, cep, numero, complemento) VALUES (1, 'Rua André Gallo', '86540000', '101', 'ap 104 bl 4');

-- -----------------------------------------------------
-- Data for table public.Usuario
-- -----------------------------------------------------
INSERT INTO public.Usuario (id, email, senha, nome, avatar, cpf_cnpj, renda, tipo_renda,endereco_id)
                    VALUES (1, 'brunoliveirabatista@gmail.com', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'Bruno Batista', '', '', 1650.00, '',1);


-- -----------------------------------------------------
-- Data for table public.Status
-- -----------------------------------------------------
INSERT INTO public.Status (id, nome) VALUES (1, 'ABERTO');
INSERT INTO public.Status (id, nome) VALUES (2, 'FINALIZADO');
INSERT INTO public.Status (id, nome) VALUES (0, 'CANCELADO');

-- -----------------------------------------------------
-- Data for table public.Tipo
-- -----------------------------------------------------
INSERT INTO public.Tipo (id, nome) VALUES (1, 'CONTINUO');
INSERT INTO public.Tipo (id, nome) VALUES (2, 'PARCELADO');