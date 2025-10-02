CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_EPS_MP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_EPS_MP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_eps_mp_time
    ON public."TB_DATA_ST_EPS_MP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_EPS_MP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_EPS_MP".id
    IS 'nextval(''"TB_DATA_ST_EPS_MP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_SALT_HP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_SALT_HP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_salt_hp_time
    ON public."TB_DATA_ST_SALT_HP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_SALT_HP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_SALT_HP".id
    IS 'nextval(''"TB_DATA_ST_SALT_HP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_SALT_LP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_SALT_LP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_salt_lp_time
    ON public."TB_DATA_ST_SALT_LP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_SALT_LP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_SALT_LP".id
    IS 'nextval(''"TB_DATA_ST_SALT_LP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_KPC_FR_MP2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_KPC_FR_MP2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_kpc_fr_mp2_time
    ON public."TB_DATA_ST_KPC_FR_MP2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_KPC_FR_MP2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_KPC_FR_MP2".id
    IS 'nextval(''"TB_DATA_ST_KPC_FR_MP2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_PR_MP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_PR_MP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_pr_mp_time
    ON public."TB_DATA_ST_PR_MP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_PR_MP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_PR_MP".id
    IS 'nextval(''"TB_DATA_ST_PR_MP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_PR_LP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_PR_LP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_pr_lp_time
    ON public."TB_DATA_ST_PR_LP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_PR_LP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_PR_LP".id
    IS 'nextval(''"TB_DATA_ST_PR_LP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_PA_FR_LP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_PA_FR_LP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_pa_fr_lp_time
    ON public."TB_DATA_ST_PA_FR_LP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_PA_FR_LP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_PA_FR_LP".id
    IS 'nextval(''"TB_DATA_ST_PA_FR_LP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_PA_HP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_PA_HP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_pa_hp_time
    ON public."TB_DATA_ST_PA_HP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_PA_HP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_PA_HP".id
    IS 'nextval(''"TB_DATA_ST_PA_HP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_SM_MP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_SM_MP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_sm_mp_time
    ON public."TB_DATA_ST_SM_MP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_SM_MP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_SM_MP".id
    IS 'nextval(''"TB_DATA_ST_SM_MP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_PSM_LP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_PSM_LP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_psm_lp_time
    ON public."TB_DATA_ST_PSM_LP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_PSM_LP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_PSM_LP".id
    IS 'nextval(''"TB_DATA_ST_PSM_LP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_PSM_MP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_PSM_MP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_psm_mp_time
    ON public."TB_DATA_ST_PSM_MP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_PSM_MP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_PSM_MP".id
    IS 'nextval(''"TB_DATA_ST_PSM_MP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_HX_LP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_HX_LP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_hx_lp_time
    ON public."TB_DATA_ST_HX_LP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_HX_LP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_HX_LP".id
    IS 'nextval(''"TB_DATA_ST_HX_LP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_HX_FW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_HX_FW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_hx_fw_time
    ON public."TB_DATA_WA_HX_FW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_HX_FW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_HX_FW".id
    IS 'nextval(''"TB_DATA_WA_HX_FW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_PP_PRI"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_PP_PRI_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_pp_pri_time
    ON public."TB_DATA_ST_PP_PRI" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_PP_PRI"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_PP_PRI".id
    IS 'nextval(''"TB_DATA_ST_PP_PRI_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_PP_MP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_PP_MP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_pp_mp_time
    ON public."TB_DATA_ST_PP_MP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_PP_MP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_PP_MP".id
    IS 'nextval(''"TB_DATA_ST_PP_MP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_PP_LP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_PP_LP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_pp_lp_time
    ON public."TB_DATA_ST_PP_LP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_PP_LP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_PP_LP".id
    IS 'nextval(''"TB_DATA_ST_PP_LP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_PE_HP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_PE_HP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_pe_hp_time
    ON public."TB_DATA_ST_PE_HP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_PE_HP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_PE_HP".id
    IS 'nextval(''"TB_DATA_ST_PE_HP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_SBR_MP2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_SBR_MP2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_sbr_mp2_time
    ON public."TB_DATA_ST_SBR_MP2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_SBR_MP2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_SBR_MP2".id
    IS 'nextval(''"TB_DATA_ST_SBR_MP2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_SBR_MP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_SBR_MP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_sbr_mp_time
    ON public."TB_DATA_ST_SBR_MP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_SBR_MP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_SBR_MP".id
    IS 'nextval(''"TB_DATA_ST_SBR_MP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_SBR_LP1"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_SBR_LP1_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_sbr_lp1_time
    ON public."TB_DATA_ST_SBR_LP1" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_SBR_LP1"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_SBR_LP1".id
    IS 'nextval(''"TB_DATA_ST_SBR_LP1_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_AA_MP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_AA_MP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_aa_mp_time
    ON public."TB_DATA_ST_AA_MP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_AA_MP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_AA_MP".id
    IS 'nextval(''"TB_DATA_ST_AA_MP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_AA_LP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_AA_LP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_aa_lp_time
    ON public."TB_DATA_ST_AA_LP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_AA_LP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_AA_LP".id
    IS 'nextval(''"TB_DATA_ST_AA_LP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_BP_HP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_BP_HP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_bp_hp_time
    ON public."TB_DATA_ST_BP_HP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_BP_HP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_BP_HP".id
    IS 'nextval(''"TB_DATA_ST_BP_HP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_SBR_LP2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_SBR_LP2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_sbr_lp2_time
    ON public."TB_DATA_ST_SBR_LP2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_SBR_LP2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_SBR_LP2".id
    IS 'nextval(''"TB_DATA_ST_SBR_LP2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_CL_HP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_CL_HP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_cl_hp_time
    ON public."TB_DATA_ST_CL_HP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_CL_HP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_CL_HP".id
    IS 'nextval(''"TB_DATA_ST_CL_HP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_PE_DW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_PE_DW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_pe_dw_time
    ON public."TB_DATA_WA_PE_DW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_PE_DW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_PE_DW".id
    IS 'nextval(''"TB_DATA_WA_PE_DW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_AA_FR_DW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_AA_FR_DW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_aa_fr_dw_time
    ON public."TB_DATA_ST_AA_FR_DW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_AA_FR_DW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_AA_FR_DW".id
    IS 'nextval(''"TB_DATA_ST_AA_FR_DW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_MA_FR_HP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_MA_FR_HP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_ma_fr_hp_time
    ON public."TB_DATA_ST_MA_FR_HP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_MA_FR_HP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_MA_FR_HP".id
    IS 'nextval(''"TB_DATA_ST_MA_FR_HP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_TPA_FR_MP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_TPA_FR_MP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_tpa_fr_mp_time
    ON public."TB_DATA_ST_TPA_FR_MP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_TPA_FR_MP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_TPA_FR_MP".id
    IS 'nextval(''"TB_DATA_ST_TPA_FR_MP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_TPA_FR_LP1"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_TPA_FR_LP1_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_tpa_fr_lp1_time
    ON public."TB_DATA_ST_TPA_FR_LP1" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_TPA_FR_LP1"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_TPA_FR_LP1".id
    IS 'nextval(''"TB_DATA_ST_TPA_FR_LP1_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_TPA_FR_LP2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_TPA_FR_LP2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_tpa_fr_lp2_time
    ON public."TB_DATA_ST_TPA_FR_LP2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_TPA_FR_LP2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_TPA_FR_LP2".id
    IS 'nextval(''"TB_DATA_ST_TPA_FR_LP2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_TPA_FW2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_TPA_FW2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_tpa_fw2_time
    ON public."TB_DATA_WA_TPA_FW2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_TPA_FW2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_TPA_FW2".id
    IS 'nextval(''"TB_DATA_WA_TPA_FW2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_AB_DW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_AB_DW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_ab_dw_time
    ON public."TB_DATA_WA_AB_DW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_AB_DW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_AB_DW".id
    IS 'nextval(''"TB_DATA_WA_AB_DW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_MA_FW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_MA_FW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_ma_fw_time
    ON public."TB_DATA_WA_MA_FW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_MA_FW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_MA_FW".id
    IS 'nextval(''"TB_DATA_WA_MA_FW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_PR_FW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_PR_FW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_pr_fw_time
    ON public."TB_DATA_WA_PR_FW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_PR_FW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_PR_FW".id
    IS 'nextval(''"TB_DATA_WA_PR_FW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_SM_FW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_SM_FW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_sm_fw_time
    ON public."TB_DATA_WA_SM_FW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_SM_FW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_SM_FW".id
    IS 'nextval(''"TB_DATA_WA_SM_FW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_TPA_FW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_TPA_FW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_tpa_fw_time
    ON public."TB_DATA_WA_TPA_FW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_TPA_FW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_TPA_FW".id
    IS 'nextval(''"TB_DATA_WA_TPA_FW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_SALT_FW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_SALT_FW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_salt_fw_time
    ON public."TB_DATA_WA_SALT_FW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_SALT_FW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_SALT_FW".id
    IS 'nextval(''"TB_DATA_WA_SALT_FW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_SM_DW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_SM_DW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_sm_dw_time
    ON public."TB_DATA_WA_SM_DW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_SM_DW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_SM_DW".id
    IS 'nextval(''"TB_DATA_WA_SM_DW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_MA_DW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_MA_DW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_ma_dw_time
    ON public."TB_DATA_WA_MA_DW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_MA_DW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_MA_DW".id
    IS 'nextval(''"TB_DATA_WA_MA_DW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_BP_FR_LP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_BP_FR_LP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_bp_fr_lp_time
    ON public."TB_DATA_ST_BP_FR_LP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_BP_FR_LP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_BP_FR_LP".id
    IS 'nextval(''"TB_DATA_ST_BP_FR_LP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_PE_FW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_PE_FW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_pe_fw_time
    ON public."TB_DATA_WA_PE_FW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_PE_FW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_PE_FW".id
    IS 'nextval(''"TB_DATA_WA_PE_FW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_AA_FW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_AA_FW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_aa_fw_time
    ON public."TB_DATA_WA_AA_FW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_AA_FW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_AA_FW".id
    IS 'nextval(''"TB_DATA_WA_AA_FW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_SALT_DW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_SALT_DW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_salt_dw_time
    ON public."TB_DATA_WA_SALT_DW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_SALT_DW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_SALT_DW".id
    IS 'nextval(''"TB_DATA_WA_SALT_DW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_SKN_DW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_SKN_DW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_skn_dw_time
    ON public."TB_DATA_WA_SKN_DW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_SKN_DW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_SKN_DW".id
    IS 'nextval(''"TB_DATA_WA_SKN_DW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_KPC_FR_MP4"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_KPC_FR_MP4_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_kpc_fr_mp4_time
    ON public."TB_DATA_ST_KPC_FR_MP4" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_KPC_FR_MP4"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_KPC_FR_MP4".id
    IS 'nextval(''"TB_DATA_ST_KPC_FR_MP4_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_PSM_DW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_PSM_DW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_psm_dw_time
    ON public."TB_DATA_WA_PSM_DW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_PSM_DW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_PSM_DW".id
    IS 'nextval(''"TB_DATA_WA_PSM_DW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_SKN_FW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_SKN_FW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_skn_fw_time
    ON public."TB_DATA_WA_SKN_FW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_SKN_FW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_SKN_FW".id
    IS 'nextval(''"TB_DATA_WA_SKN_FW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_PP_FW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_PP_FW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_pp_fw_time
    ON public."TB_DATA_WA_PP_FW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_PP_FW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_PP_FW".id
    IS 'nextval(''"TB_DATA_WA_PP_FW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_AN_FW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_AN_FW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_an_fw_time
    ON public."TB_DATA_WA_AN_FW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_AN_FW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_AN_FW".id
    IS 'nextval(''"TB_DATA_WA_AN_FW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_PA_FW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_PA_FW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_pa_fw_time
    ON public."TB_DATA_WA_PA_FW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_PA_FW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_PA_FW".id
    IS 'nextval(''"TB_DATA_WA_PA_FW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_AA_DW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_AA_DW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_aa_dw_time
    ON public."TB_DATA_WA_AA_DW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_AA_DW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_AA_DW".id
    IS 'nextval(''"TB_DATA_WA_AA_DW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_PP_DW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_PP_DW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_pp_dw_time
    ON public."TB_DATA_WA_PP_DW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_PP_DW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_PP_DW".id
    IS 'nextval(''"TB_DATA_WA_PP_DW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_PA_DW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_PA_DW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_pa_dw_time
    ON public."TB_DATA_WA_PA_DW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_PA_DW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_PA_DW".id
    IS 'nextval(''"TB_DATA_WA_PA_DW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_AN_DW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_AN_DW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_an_dw_time
    ON public."TB_DATA_WA_AN_DW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_AN_DW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_AN_DW".id
    IS 'nextval(''"TB_DATA_WA_AN_DW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_SFC_HP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_SFC_HP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_sfc_hp_time
    ON public."TB_DATA_ST_SFC_HP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_SFC_HP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_SFC_HP".id
    IS 'nextval(''"TB_DATA_ST_SFC_HP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_TPA_FR_SFC2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_TPA_FR_SFC2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_tpa_fr_sfc2_time
    ON public."TB_DATA_ST_TPA_FR_SFC2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_TPA_FR_SFC2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_TPA_FR_SFC2".id
    IS 'nextval(''"TB_DATA_ST_TPA_FR_SFC2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_TPA_FR_SFC1"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_TPA_FR_SFC1_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_tpa_fr_sfc1_time
    ON public."TB_DATA_ST_TPA_FR_SFC1" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_TPA_FR_SFC1"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_TPA_FR_SFC1".id
    IS 'nextval(''"TB_DATA_ST_TPA_FR_SFC1_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_AA_FR_FW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_AA_FR_FW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_aa_fr_fw_time
    ON public."TB_DATA_ST_AA_FR_FW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_AA_FR_FW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_AA_FR_FW".id
    IS 'nextval(''"TB_DATA_ST_AA_FR_FW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_SKN_HP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_SKN_HP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_skn_hp_time
    ON public."TB_DATA_ST_SKN_HP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_SKN_HP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_SKN_HP".id
    IS 'nextval(''"TB_DATA_ST_SKN_HP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_SKP_HP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_SKP_HP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_skp_hp_time
    ON public."TB_DATA_ST_SKP_HP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_SKP_HP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_SKP_HP".id
    IS 'nextval(''"TB_DATA_ST_SKP_HP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_SKP_PRI"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_SKP_PRI_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_skp_pri_time
    ON public."TB_DATA_ST_SKP_PRI" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_SKP_PRI"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_SKP_PRI".id
    IS 'nextval(''"TB_DATA_ST_SKP_PRI_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_WA_AB_FW"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_WA_AB_FW_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_wa_ab_fw_time
    ON public."TB_DATA_WA_AB_FW" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_WA_AB_FW"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_WA_AB_FW".id
    IS 'nextval(''"TB_DATA_WA_AB_FW_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_UNID_HP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_UNID_HP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_unid_hp_time
    ON public."TB_DATA_ST_UNID_HP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_UNID_HP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_UNID_HP".id
    IS 'nextval(''"TB_DATA_ST_UNID_HP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_PSM_LP2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_PSM_LP2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_psm_lp2_time
    ON public."TB_DATA_ST_PSM_LP2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_PSM_LP2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_PSM_LP2".id
    IS 'nextval(''"TB_DATA_ST_PSM_LP2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_BK_MP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_BK_MP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_bk_mp_time
    ON public."TB_DATA_ST_BK_MP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_BK_MP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_BK_MP".id
    IS 'nextval(''"TB_DATA_ST_BK_MP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_STAC_HP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_STAC_HP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_stac_hp_time
    ON public."TB_DATA_ST_STAC_HP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_STAC_HP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_STAC_HP".id
    IS 'nextval(''"TB_DATA_ST_STAC_HP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_SFC_MP1"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_SFC_MP1_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_sfc_mp1_time
    ON public."TB_DATA_ST_SFC_MP1" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_SFC_MP1"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_SFC_MP1".id
    IS 'nextval(''"TB_DATA_ST_SFC_MP1_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_SFC_MP2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_SFC_MP2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_sfc_mp2_time
    ON public."TB_DATA_ST_SFC_MP2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_SFC_MP2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_SFC_MP2".id
    IS 'nextval(''"TB_DATA_ST_SFC_MP2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_HCC2_MP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_HCC2_MP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_hcc2_mp_time
    ON public."TB_DATA_ST_HCC2_MP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_HCC2_MP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_HCC2_MP".id
    IS 'nextval(''"TB_DATA_ST_HCC2_MP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_TPA_FR_LP3"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_TPA_FR_LP3_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_tpa_fr_lp3_time
    ON public."TB_DATA_ST_TPA_FR_LP3" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_TPA_FR_LP3"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_TPA_FR_LP3".id
    IS 'nextval(''"TB_DATA_ST_TPA_FR_LP3_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_TPA_FR_MP2"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_TPA_FR_MP2_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_tpa_fr_mp2_time
    ON public."TB_DATA_ST_TPA_FR_MP2" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_TPA_FR_MP2"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_TPA_FR_MP2".id
    IS 'nextval(''"TB_DATA_ST_TPA_FR_MP2_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_TPA_FR_MP3"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_TPA_FR_MP3_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_tpa_fr_mp3_time
    ON public."TB_DATA_ST_TPA_FR_MP3" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_TPA_FR_MP3"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_TPA_FR_MP3".id
    IS 'nextval(''"TB_DATA_ST_TPA_FR_MP3_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_TPA_FR_MP4"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_TPA_FR_MP4_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_tpa_fr_mp4_time
    ON public."TB_DATA_ST_TPA_FR_MP4" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_TPA_FR_MP4"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_TPA_FR_MP4".id
    IS 'nextval(''"TB_DATA_ST_TPA_FR_MP4_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_SFCM_MP"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_SFCM_MP_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_sfcm_mp_time
    ON public."TB_DATA_ST_SFCM_MP" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_SFCM_MP"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_SFCM_MP".id
    IS 'nextval(''"TB_DATA_ST_SFCM_MP_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_USA_FR_PRI"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_USA_FR_PRI_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_usa_fr_pri_time
    ON public."TB_DATA_ST_USA_FR_PRI" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_USA_FR_PRI"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_USA_FR_PRI".id
    IS 'nextval(''"TB_DATA_ST_USA_FR_PRI_SEQ"''::regclass)';


CREATE TABLE IF NOT EXISTS public."TB_DATA_ST_HP_FR_CL"
(
    id integer NOT NULL DEFAULT nextval('"TB_DATA_ST_HP_FR_CL_SEQ"'::regclass),
    num_id integer,
    name character varying COLLATE pg_catalog."default",
    value character varying COLLATE pg_catalog."default",
    "time" timestamp with time zone
)
TABLESPACE pg_default;

CREATE INDEX idx_tb_data_st_hp_fr_cl_time
    ON public."TB_DATA_ST_HP_FR_CL" ("time" DESC);

ALTER TABLE IF EXISTS public."TB_DATA_ST_HP_FR_CL"
    OWNER TO postgres;

COMMENT ON COLUMN public."TB_DATA_ST_HP_FR_CL".id
    IS 'nextval(''"TB_DATA_ST_HP_FR_CL_SEQ"''::regclass)';

