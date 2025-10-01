CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_SBR1"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_SBR1_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_sbr1_time
    ON public."TB_DATA_PW_SBR1" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_SBR1"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_SBR1".id
    IS 'nextval(''"TB_DATA_PW_SBR1_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_PSM2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_PSM2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_psm2_time
    ON public."TB_DATA_PW_PSM2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_PSM2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_PSM2".id
    IS 'nextval(''"TB_DATA_PW_PSM2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_CL2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_CL2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_cl2_time
    ON public."TB_DATA_PW_CL2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_CL2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_CL2".id
    IS 'nextval(''"TB_DATA_PW_CL2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_TPA2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_TPA2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_tpa2_time
    ON public."TB_DATA_PW_TPA2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_TPA2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_TPA2".id
    IS 'nextval(''"TB_DATA_PW_TPA2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_AB2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_AB2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_ab2_time
    ON public."TB_DATA_PW_AB2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_AB2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_AB2".id
    IS 'nextval(''"TB_DATA_PW_AB2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_PP_EPK5"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_PP_EPK5_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_pp_epk5_time
    ON public."TB_DATA_PW_PP_EPK5" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_PP_EPK5"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_PP_EPK5".id
    IS 'nextval(''"TB_DATA_PW_PP_EPK5_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_PP6"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_PP6_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_pp6_time
    ON public."TB_DATA_PW_PP6" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_PP6"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_PP6".id
    IS 'nextval(''"TB_DATA_PW_PP6_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_PP7"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_PP7_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_pp7_time
    ON public."TB_DATA_PW_PP7" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_PP7"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_PP7".id
    IS 'nextval(''"TB_DATA_PW_PP7_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_HX"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_HX_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_hx_time
    ON public."TB_DATA_PW_HX" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_HX"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_HX".id
    IS 'nextval(''"TB_DATA_PW_HX_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_SKN"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_SKN_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_skn_time
    ON public."TB_DATA_PW_SKN" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_SKN"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_SKN".id
    IS 'nextval(''"TB_DATA_PW_SKN_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_PP1"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_PP1_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_pp1_time
    ON public."TB_DATA_PW_PP1" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_PP1"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_PP1".id
    IS 'nextval(''"TB_DATA_PW_PP1_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_PSM1"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_PSM1_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_psm1_time
    ON public."TB_DATA_PW_PSM1" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_PSM1"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_PSM1".id
    IS 'nextval(''"TB_DATA_PW_PSM1_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_PR"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_PR_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_pr_time
    ON public."TB_DATA_PW_PR" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_PR"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_PR".id
    IS 'nextval(''"TB_DATA_PW_PR_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_PE1"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_PE1_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_pe1_time
    ON public."TB_DATA_PW_PE1" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_PE1"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_PE1".id
    IS 'nextval(''"TB_DATA_PW_PE1_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_BP2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_BP2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_bp2_time
    ON public."TB_DATA_PW_BP2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_BP2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_BP2".id
    IS 'nextval(''"TB_DATA_PW_BP2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_PP_AEK4"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_PP_AEK4_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_pp_aek4_time
    ON public."TB_DATA_PW_PP_AEK4" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_PP_AEK4"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_PP_AEK4".id
    IS 'nextval(''"TB_DATA_PW_PP_AEK4_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_EPS1"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_EPS1_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_eps1_time
    ON public."TB_DATA_PW_EPS1" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_EPS1"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_EPS1".id
    IS 'nextval(''"TB_DATA_PW_EPS1_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_PE2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_PE2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_pe2_time
    ON public."TB_DATA_PW_PE2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_PE2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_PE2".id
    IS 'nextval(''"TB_DATA_PW_PE2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_AN2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_AN2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_an2_time
    ON public."TB_DATA_PW_AN2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_AN2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_AN2".id
    IS 'nextval(''"TB_DATA_PW_AN2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_UNID3"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_UNID3_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_unid3_time
    ON public."TB_DATA_PW_UNID3" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_UNID3"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_UNID3".id
    IS 'nextval(''"TB_DATA_PW_UNID3_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_HJ_TL"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_HJ_TL_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_hj_tl_time
    ON public."TB_DATA_PW_HJ_TL" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_HJ_TL"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_HJ_TL".id
    IS 'nextval(''"TB_DATA_PW_HJ_TL_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_PSM4"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_PSM4_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_psm4_time
    ON public."TB_DATA_PW_PSM4" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_PSM4"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_PSM4".id
    IS 'nextval(''"TB_DATA_PW_PSM4_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_PA2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_PA2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_pa2_time
    ON public."TB_DATA_PW_PA2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_PA2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_PA2".id
    IS 'nextval(''"TB_DATA_PW_PA2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_PP_PK3"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_PP_PK3_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_pp_pk3_time
    ON public."TB_DATA_PW_PP_PK3" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_PP_PK3"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_PP_PK3".id
    IS 'nextval(''"TB_DATA_PW_PP_PK3_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_SBR2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_SBR2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_sbr2_time
    ON public."TB_DATA_PW_SBR2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_SBR2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_SBR2".id
    IS 'nextval(''"TB_DATA_PW_SBR2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_UNID2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_UNID2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_unid2_time
    ON public."TB_DATA_PW_UNID2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_UNID2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_UNID2".id
    IS 'nextval(''"TB_DATA_PW_UNID2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_BP1"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_BP1_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_bp1_time
    ON public."TB_DATA_PW_BP1" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_BP1"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_BP1".id
    IS 'nextval(''"TB_DATA_PW_BP1_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_PSM3"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_PSM3_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_psm3_time
    ON public."TB_DATA_PW_PSM3" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_PSM3"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_PSM3".id
    IS 'nextval(''"TB_DATA_PW_PSM3_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_TPA3"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_TPA3_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_tpa3_time
    ON public."TB_DATA_PW_TPA3" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_TPA3"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_TPA3".id
    IS 'nextval(''"TB_DATA_PW_TPA3_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_PET_KPC2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_PET_KPC2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_pet_kpc2_time
    ON public."TB_DATA_PW_PET_KPC2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_PET_KPC2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_PET_KPC2".id
    IS 'nextval(''"TB_DATA_PW_PET_KPC2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_EPS2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_EPS2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_eps2_time
    ON public."TB_DATA_PW_EPS2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_EPS2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_EPS2".id
    IS 'nextval(''"TB_DATA_PW_EPS2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_SUKJITL01"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_SUKJITL01_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_sukjitl01_time
    ON public."TB_DATA_PW_SUKJITL01" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_SUKJITL01"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_SUKJITL01".id
    IS 'nextval(''"TB_DATA_PW_SUKJITL01_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_SUKJITL02"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_SUKJITL02_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_sukjitl02_time
    ON public."TB_DATA_PW_SUKJITL02" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_SUKJITL02"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_SUKJITL02".id
    IS 'nextval(''"TB_DATA_PW_SUKJITL02_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_SBR3"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_SBR3_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_sbr3_time
    ON public."TB_DATA_PW_SBR3" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_SBR3"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_SBR3".id
    IS 'nextval(''"TB_DATA_PW_SBR3_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_CL3"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_CL3_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_cl3_time
    ON public."TB_DATA_PW_CL3" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_CL3"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_CL3".id
    IS 'nextval(''"TB_DATA_PW_CL3_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_SALT"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_SALT_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_salt_time
    ON public."TB_DATA_PW_SALT" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_SALT"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_SALT".id
    IS 'nextval(''"TB_DATA_PW_SALT_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_AN3"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_AN3_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_an3_time
    ON public."TB_DATA_PW_AN3" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_AN3"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_AN3".id
    IS 'nextval(''"TB_DATA_PW_AN3_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_PET2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_PET2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_pet2_time
    ON public."TB_DATA_PW_PET2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_PET2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_PET2".id
    IS 'nextval(''"TB_DATA_PW_PET2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_AB1"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_AB1_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_ab1_time
    ON public."TB_DATA_PW_AB1" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_AB1"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_AB1".id
    IS 'nextval(''"TB_DATA_PW_AB1_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_PSM5"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_PSM5_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_psm5_time
    ON public."TB_DATA_PW_PSM5" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_PSM5"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_PSM5".id
    IS 'nextval(''"TB_DATA_PW_PSM5_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_SM"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_SM_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_sm_time
    ON public."TB_DATA_PW_SM" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_SM"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_SM".id
    IS 'nextval(''"TB_DATA_PW_SM_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_AA"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_AA_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_aa_time
    ON public."TB_DATA_PW_AA" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_AA"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_AA".id
    IS 'nextval(''"TB_DATA_PW_AA_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_TPA1"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_TPA1_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_tpa1_time
    ON public."TB_DATA_PW_TPA1" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_TPA1"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_TPA1".id
    IS 'nextval(''"TB_DATA_PW_TPA1_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_UNID1"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_UNID1_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_unid1_time
    ON public."TB_DATA_PW_UNID1" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_UNID1"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_UNID1".id
    IS 'nextval(''"TB_DATA_PW_UNID1_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_PW_MA"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_PW_MA_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)

TABLESPACE pg_default;

CREATE INDEX idx_tb_data_pw_ma_time
    ON public."TB_DATA_PW_MA" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_PW_MA"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_PW_MA".id
    IS 'nextval(''"TB_DATA_PW_MA_SEQ"''::regclass)';
