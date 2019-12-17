--
-- PostgreSQL database dump
--

-- Dumped from database version 11.6
-- Dumped by pg_dump version 11.6

-- Started on 2019-12-12 14:50:35

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

DROP DATABASE "SynapseIdentity";
--
-- TOC entry 3157 (class 1262 OID 39649)
-- Name: SynapseIdentity; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE "SynapseIdentity" WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'English_United States.1252' LC_CTYPE = 'English_United States.1252';


ALTER DATABASE "SynapseIdentity" OWNER TO "postgres";

\connect "SynapseIdentity"

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 2 (class 3079 OID 39650)
-- Name: pg_stat_statements; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS "pg_stat_statements" WITH SCHEMA "public";


--
-- TOC entry 3158 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION "pg_stat_statements"; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION "pg_stat_statements" IS 'track execution statistics of all SQL statements executed';


SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 198 (class 1259 OID 39657)
-- Name: ApiClaims; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."ApiClaims" (
    "Id" integer NOT NULL,
    "Type" character varying(200) NOT NULL,
    "ApiResourceId" integer NOT NULL
);


ALTER TABLE "public"."ApiClaims" OWNER TO "postgres";

--
-- TOC entry 199 (class 1259 OID 39660)
-- Name: ApiClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."ApiClaims_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."ApiClaims_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3159 (class 0 OID 0)
-- Dependencies: 199
-- Name: ApiClaims_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."ApiClaims_Id_seq" OWNED BY "public"."ApiClaims"."Id";


--
-- TOC entry 200 (class 1259 OID 39662)
-- Name: ApiResources; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."ApiResources" (
    "Id" integer NOT NULL,
    "Enabled" boolean NOT NULL,
    "Name" character varying(200) NOT NULL,
    "DisplayName" character varying(200),
    "Description" character varying(1000)
);


ALTER TABLE "public"."ApiResources" OWNER TO "postgres";

--
-- TOC entry 201 (class 1259 OID 39668)
-- Name: ApiResources_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."ApiResources_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."ApiResources_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3160 (class 0 OID 0)
-- Dependencies: 201
-- Name: ApiResources_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."ApiResources_Id_seq" OWNED BY "public"."ApiResources"."Id";


--
-- TOC entry 202 (class 1259 OID 39670)
-- Name: ApiScopeClaims; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."ApiScopeClaims" (
    "Id" integer NOT NULL,
    "Type" character varying(200) NOT NULL,
    "ApiScopeId" integer NOT NULL
);


ALTER TABLE "public"."ApiScopeClaims" OWNER TO "postgres";

--
-- TOC entry 203 (class 1259 OID 39673)
-- Name: ApiScopeClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."ApiScopeClaims_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."ApiScopeClaims_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3161 (class 0 OID 0)
-- Dependencies: 203
-- Name: ApiScopeClaims_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."ApiScopeClaims_Id_seq" OWNED BY "public"."ApiScopeClaims"."Id";


--
-- TOC entry 204 (class 1259 OID 39675)
-- Name: ApiScopePermissions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."ApiScopePermissions" (
    "ApiScopeId" integer NOT NULL,
    "RoleId" "text" NOT NULL,
    "Id" "text" NOT NULL
);


ALTER TABLE "public"."ApiScopePermissions" OWNER TO "postgres";

--
-- TOC entry 205 (class 1259 OID 39681)
-- Name: ApiScopes; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."ApiScopes" (
    "Id" integer NOT NULL,
    "Name" character varying(200) NOT NULL,
    "DisplayName" character varying(200),
    "Description" character varying(1000),
    "Required" boolean NOT NULL,
    "Emphasize" boolean NOT NULL,
    "ShowInDiscoveryDocument" boolean NOT NULL,
    "ApiResourceId" integer NOT NULL
);


ALTER TABLE "public"."ApiScopes" OWNER TO "postgres";

--
-- TOC entry 206 (class 1259 OID 39687)
-- Name: ApiScopes_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."ApiScopes_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."ApiScopes_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3162 (class 0 OID 0)
-- Dependencies: 206
-- Name: ApiScopes_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."ApiScopes_Id_seq" OWNED BY "public"."ApiScopes"."Id";


--
-- TOC entry 207 (class 1259 OID 39689)
-- Name: ApiSecrets; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."ApiSecrets" (
    "Id" integer NOT NULL,
    "Description" character varying(1000),
    "Value" character varying(2000),
    "Expiration" timestamp without time zone,
    "Type" character varying(250),
    "ApiResourceId" integer NOT NULL
);


ALTER TABLE "public"."ApiSecrets" OWNER TO "postgres";

--
-- TOC entry 208 (class 1259 OID 39695)
-- Name: ApiSecrets_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."ApiSecrets_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."ApiSecrets_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3163 (class 0 OID 0)
-- Dependencies: 208
-- Name: ApiSecrets_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."ApiSecrets_Id_seq" OWNED BY "public"."ApiSecrets"."Id";


--
-- TOC entry 209 (class 1259 OID 39697)
-- Name: AspNetRoleClaims; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."AspNetRoleClaims" (
    "Id" integer NOT NULL,
    "RoleId" "text" NOT NULL,
    "ClaimType" "text",
    "ClaimValue" "text"
);


ALTER TABLE "public"."AspNetRoleClaims" OWNER TO "postgres";

--
-- TOC entry 210 (class 1259 OID 39703)
-- Name: AspNetRoleClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."AspNetRoleClaims_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."AspNetRoleClaims_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3164 (class 0 OID 0)
-- Dependencies: 210
-- Name: AspNetRoleClaims_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."AspNetRoleClaims_Id_seq" OWNED BY "public"."AspNetRoleClaims"."Id";


--
-- TOC entry 211 (class 1259 OID 39705)
-- Name: AspNetRoles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."AspNetRoles" (
    "Id" "text" NOT NULL,
    "Name" character varying(256),
    "NormalizedName" character varying(256),
    "ConcurrencyStamp" "text"
);


ALTER TABLE "public"."AspNetRoles" OWNER TO "postgres";

--
-- TOC entry 212 (class 1259 OID 39711)
-- Name: AspNetUserClaims; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."AspNetUserClaims" (
    "Id" integer NOT NULL,
    "UserId" "text" NOT NULL,
    "ClaimType" "text",
    "ClaimValue" "text"
);


ALTER TABLE "public"."AspNetUserClaims" OWNER TO "postgres";

--
-- TOC entry 213 (class 1259 OID 39717)
-- Name: AspNetUserClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."AspNetUserClaims_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."AspNetUserClaims_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3165 (class 0 OID 0)
-- Dependencies: 213
-- Name: AspNetUserClaims_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."AspNetUserClaims_Id_seq" OWNED BY "public"."AspNetUserClaims"."Id";


--
-- TOC entry 214 (class 1259 OID 39719)
-- Name: AspNetUserLogins; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."AspNetUserLogins" (
    "LoginProvider" "text" NOT NULL,
    "ProviderKey" "text" NOT NULL,
    "ProviderDisplayName" "text",
    "UserId" "text" NOT NULL
);


ALTER TABLE "public"."AspNetUserLogins" OWNER TO "postgres";

--
-- TOC entry 215 (class 1259 OID 39725)
-- Name: AspNetUserRoles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."AspNetUserRoles" (
    "UserId" "text" NOT NULL,
    "RoleId" "text" NOT NULL
);


ALTER TABLE "public"."AspNetUserRoles" OWNER TO "postgres";

--
-- TOC entry 216 (class 1259 OID 39731)
-- Name: AspNetUserTokens; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."AspNetUserTokens" (
    "UserId" "text" NOT NULL,
    "LoginProvider" "text" NOT NULL,
    "Name" "text" NOT NULL,
    "Value" "text"
);


ALTER TABLE "public"."AspNetUserTokens" OWNER TO "postgres";

--
-- TOC entry 217 (class 1259 OID 39737)
-- Name: AspNetUsers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."AspNetUsers" (
    "Id" "text" NOT NULL,
    "UserName" character varying(256),
    "NormalizedUserName" character varying(256),
    "Email" character varying(256),
    "NormalizedEmail" character varying(256),
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" "text",
    "SecurityStamp" "text",
    "ConcurrencyStamp" "text",
    "PhoneNumber" "text",
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL
);


ALTER TABLE "public"."AspNetUsers" OWNER TO "postgres";

--
-- TOC entry 218 (class 1259 OID 39743)
-- Name: ClientClaims; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."ClientClaims" (
    "Id" integer NOT NULL,
    "Type" character varying(250) NOT NULL,
    "Value" character varying(250) NOT NULL,
    "ClientId" integer NOT NULL
);


ALTER TABLE "public"."ClientClaims" OWNER TO "postgres";

--
-- TOC entry 219 (class 1259 OID 39749)
-- Name: ClientClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."ClientClaims_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."ClientClaims_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3166 (class 0 OID 0)
-- Dependencies: 219
-- Name: ClientClaims_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."ClientClaims_Id_seq" OWNED BY "public"."ClientClaims"."Id";


--
-- TOC entry 220 (class 1259 OID 39751)
-- Name: ClientCorsOrigins; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."ClientCorsOrigins" (
    "Id" integer NOT NULL,
    "Origin" character varying(150) NOT NULL,
    "ClientId" integer NOT NULL
);


ALTER TABLE "public"."ClientCorsOrigins" OWNER TO "postgres";

--
-- TOC entry 221 (class 1259 OID 39754)
-- Name: ClientCorsOrigins_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."ClientCorsOrigins_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."ClientCorsOrigins_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3167 (class 0 OID 0)
-- Dependencies: 221
-- Name: ClientCorsOrigins_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."ClientCorsOrigins_Id_seq" OWNED BY "public"."ClientCorsOrigins"."Id";


--
-- TOC entry 222 (class 1259 OID 39756)
-- Name: ClientGrantTypes; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."ClientGrantTypes" (
    "Id" integer NOT NULL,
    "GrantType" character varying(250) NOT NULL,
    "ClientId" integer NOT NULL
);


ALTER TABLE "public"."ClientGrantTypes" OWNER TO "postgres";

--
-- TOC entry 223 (class 1259 OID 39759)
-- Name: ClientGrantTypes_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."ClientGrantTypes_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."ClientGrantTypes_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3168 (class 0 OID 0)
-- Dependencies: 223
-- Name: ClientGrantTypes_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."ClientGrantTypes_Id_seq" OWNED BY "public"."ClientGrantTypes"."Id";


--
-- TOC entry 224 (class 1259 OID 39761)
-- Name: ClientIdPRestrictions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."ClientIdPRestrictions" (
    "Id" integer NOT NULL,
    "Provider" character varying(200) NOT NULL,
    "ClientId" integer NOT NULL
);


ALTER TABLE "public"."ClientIdPRestrictions" OWNER TO "postgres";

--
-- TOC entry 225 (class 1259 OID 39764)
-- Name: ClientIdPRestrictions_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."ClientIdPRestrictions_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."ClientIdPRestrictions_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3169 (class 0 OID 0)
-- Dependencies: 225
-- Name: ClientIdPRestrictions_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."ClientIdPRestrictions_Id_seq" OWNED BY "public"."ClientIdPRestrictions"."Id";


--
-- TOC entry 226 (class 1259 OID 39766)
-- Name: ClientPostLogoutRedirectUris; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."ClientPostLogoutRedirectUris" (
    "Id" integer NOT NULL,
    "PostLogoutRedirectUri" character varying(2000) NOT NULL,
    "ClientId" integer NOT NULL
);


ALTER TABLE "public"."ClientPostLogoutRedirectUris" OWNER TO "postgres";

--
-- TOC entry 227 (class 1259 OID 39772)
-- Name: ClientPostLogoutRedirectUris_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."ClientPostLogoutRedirectUris_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."ClientPostLogoutRedirectUris_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3170 (class 0 OID 0)
-- Dependencies: 227
-- Name: ClientPostLogoutRedirectUris_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."ClientPostLogoutRedirectUris_Id_seq" OWNED BY "public"."ClientPostLogoutRedirectUris"."Id";


--
-- TOC entry 228 (class 1259 OID 39774)
-- Name: ClientProperties; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."ClientProperties" (
    "Id" integer NOT NULL,
    "Key" character varying(250) NOT NULL,
    "Value" character varying(2000) NOT NULL,
    "ClientId" integer NOT NULL
);


ALTER TABLE "public"."ClientProperties" OWNER TO "postgres";

--
-- TOC entry 229 (class 1259 OID 39780)
-- Name: ClientProperties_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."ClientProperties_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."ClientProperties_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3171 (class 0 OID 0)
-- Dependencies: 229
-- Name: ClientProperties_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."ClientProperties_Id_seq" OWNED BY "public"."ClientProperties"."Id";


--
-- TOC entry 230 (class 1259 OID 39782)
-- Name: ClientRedirectUris; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."ClientRedirectUris" (
    "Id" integer NOT NULL,
    "RedirectUri" character varying(2000) NOT NULL,
    "ClientId" integer NOT NULL
);


ALTER TABLE "public"."ClientRedirectUris" OWNER TO "postgres";

--
-- TOC entry 231 (class 1259 OID 39788)
-- Name: ClientRedirectUris_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."ClientRedirectUris_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."ClientRedirectUris_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3172 (class 0 OID 0)
-- Dependencies: 231
-- Name: ClientRedirectUris_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."ClientRedirectUris_Id_seq" OWNED BY "public"."ClientRedirectUris"."Id";


--
-- TOC entry 232 (class 1259 OID 39790)
-- Name: ClientScopes; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."ClientScopes" (
    "Id" integer NOT NULL,
    "Scope" character varying(200) NOT NULL,
    "ClientId" integer NOT NULL
);


ALTER TABLE "public"."ClientScopes" OWNER TO "postgres";

--
-- TOC entry 233 (class 1259 OID 39793)
-- Name: ClientScopes_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."ClientScopes_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."ClientScopes_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3173 (class 0 OID 0)
-- Dependencies: 233
-- Name: ClientScopes_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."ClientScopes_Id_seq" OWNED BY "public"."ClientScopes"."Id";


--
-- TOC entry 234 (class 1259 OID 39795)
-- Name: ClientSecrets; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."ClientSecrets" (
    "Id" integer NOT NULL,
    "Description" character varying(2000),
    "Value" character varying(2000) NOT NULL,
    "Expiration" timestamp without time zone,
    "Type" character varying(250),
    "ClientId" integer NOT NULL
);


ALTER TABLE "public"."ClientSecrets" OWNER TO "postgres";

--
-- TOC entry 235 (class 1259 OID 39801)
-- Name: ClientSecrets_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."ClientSecrets_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."ClientSecrets_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3174 (class 0 OID 0)
-- Dependencies: 235
-- Name: ClientSecrets_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."ClientSecrets_Id_seq" OWNED BY "public"."ClientSecrets"."Id";


--
-- TOC entry 236 (class 1259 OID 39803)
-- Name: Clients; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."Clients" (
    "Id" integer NOT NULL,
    "Enabled" boolean NOT NULL,
    "ClientId" character varying(200) NOT NULL,
    "ProtocolType" character varying(200) NOT NULL,
    "RequireClientSecret" boolean NOT NULL,
    "ClientName" character varying(200),
    "Description" character varying(1000),
    "ClientUri" character varying(2000),
    "LogoUri" character varying(2000),
    "RequireConsent" boolean NOT NULL,
    "AllowRememberConsent" boolean NOT NULL,
    "AlwaysIncludeUserClaimsInIdToken" boolean NOT NULL,
    "RequirePkce" boolean NOT NULL,
    "AllowPlainTextPkce" boolean NOT NULL,
    "AllowAccessTokensViaBrowser" boolean NOT NULL,
    "FrontChannelLogoutUri" character varying(2000),
    "FrontChannelLogoutSessionRequired" boolean NOT NULL,
    "BackChannelLogoutUri" character varying(2000),
    "BackChannelLogoutSessionRequired" boolean NOT NULL,
    "AllowOfflineAccess" boolean NOT NULL,
    "IdentityTokenLifetime" integer NOT NULL,
    "AccessTokenLifetime" integer NOT NULL,
    "AuthorizationCodeLifetime" integer NOT NULL,
    "ConsentLifetime" integer,
    "AbsoluteRefreshTokenLifetime" integer NOT NULL,
    "SlidingRefreshTokenLifetime" integer NOT NULL,
    "RefreshTokenUsage" integer NOT NULL,
    "UpdateAccessTokenClaimsOnRefresh" boolean NOT NULL,
    "RefreshTokenExpiration" integer NOT NULL,
    "AccessTokenType" integer NOT NULL,
    "EnableLocalLogin" boolean NOT NULL,
    "IncludeJwtId" boolean NOT NULL,
    "AlwaysSendClientClaims" boolean NOT NULL,
    "ClientClaimsPrefix" character varying(200),
    "PairWiseSubjectSalt" character varying(200)
);


ALTER TABLE "public"."Clients" OWNER TO "postgres";

--
-- TOC entry 237 (class 1259 OID 39809)
-- Name: Clients_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."Clients_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."Clients_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3175 (class 0 OID 0)
-- Dependencies: 237
-- Name: Clients_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."Clients_Id_seq" OWNED BY "public"."Clients"."Id";


--
-- TOC entry 238 (class 1259 OID 39811)
-- Name: IdentityClaims; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."IdentityClaims" (
    "Id" integer NOT NULL,
    "Type" character varying(200) NOT NULL,
    "IdentityResourceId" integer NOT NULL
);


ALTER TABLE "public"."IdentityClaims" OWNER TO "postgres";

--
-- TOC entry 239 (class 1259 OID 39814)
-- Name: IdentityClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."IdentityClaims_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."IdentityClaims_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3176 (class 0 OID 0)
-- Dependencies: 239
-- Name: IdentityClaims_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."IdentityClaims_Id_seq" OWNED BY "public"."IdentityClaims"."Id";


--
-- TOC entry 240 (class 1259 OID 39816)
-- Name: IdentityResources; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."IdentityResources" (
    "Id" integer NOT NULL,
    "Enabled" boolean NOT NULL,
    "Name" character varying(200) NOT NULL,
    "DisplayName" character varying(200),
    "Description" character varying(1000),
    "Required" boolean NOT NULL,
    "Emphasize" boolean NOT NULL,
    "ShowInDiscoveryDocument" boolean NOT NULL
);


ALTER TABLE "public"."IdentityResources" OWNER TO "postgres";

--
-- TOC entry 241 (class 1259 OID 39822)
-- Name: IdentityResources_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "public"."IdentityResources_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "public"."IdentityResources_Id_seq" OWNER TO "postgres";

--
-- TOC entry 3177 (class 0 OID 0)
-- Dependencies: 241
-- Name: IdentityResources_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "public"."IdentityResources_Id_seq" OWNED BY "public"."IdentityResources"."Id";


--
-- TOC entry 242 (class 1259 OID 39824)
-- Name: PersistedGrants; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."PersistedGrants" (
    "Key" character varying(200) NOT NULL,
    "Type" character varying(50) NOT NULL,
    "SubjectId" character varying(200),
    "ClientId" character varying(200) NOT NULL,
    "CreationTime" timestamp without time zone NOT NULL,
    "Expiration" timestamp without time zone,
    "Data" character varying(50000) NOT NULL
);


ALTER TABLE "public"."PersistedGrants" OWNER TO "postgres";

--
-- TOC entry 243 (class 1259 OID 39830)
-- Name: UserRoles_ExternalProviders; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."UserRoles_ExternalProviders" (
    "ExternalSubjectId" "text" NOT NULL,
    "Id" "text" NOT NULL,
    "Idp" "text",
    "RoleId" "text" NOT NULL
);


ALTER TABLE "public"."UserRoles_ExternalProviders" OWNER TO "postgres";

--
-- TOC entry 244 (class 1259 OID 39836)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE "public"."__EFMigrationsHistory" OWNER TO "postgres";

--
-- TOC entry 245 (class 1259 OID 39839)
-- Name: sislog; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "public"."sislog" (
    "message" "text",
    "message_template" "text",
    "level" integer,
    "timestamp" timestamp without time zone,
    "exception" "text",
    "log_event" "jsonb"
);


ALTER TABLE "public"."sislog" OWNER TO "postgres";

--
-- TOC entry 2861 (class 2604 OID 39845)
-- Name: ApiClaims Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ApiClaims" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."ApiClaims_Id_seq"'::"regclass");


--
-- TOC entry 2862 (class 2604 OID 39846)
-- Name: ApiResources Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ApiResources" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."ApiResources_Id_seq"'::"regclass");


--
-- TOC entry 2863 (class 2604 OID 39847)
-- Name: ApiScopeClaims Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ApiScopeClaims" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."ApiScopeClaims_Id_seq"'::"regclass");


--
-- TOC entry 2864 (class 2604 OID 39848)
-- Name: ApiScopes Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ApiScopes" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."ApiScopes_Id_seq"'::"regclass");


--
-- TOC entry 2865 (class 2604 OID 39849)
-- Name: ApiSecrets Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ApiSecrets" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."ApiSecrets_Id_seq"'::"regclass");


--
-- TOC entry 2866 (class 2604 OID 39850)
-- Name: AspNetRoleClaims Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."AspNetRoleClaims" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."AspNetRoleClaims_Id_seq"'::"regclass");


--
-- TOC entry 2867 (class 2604 OID 39851)
-- Name: AspNetUserClaims Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."AspNetUserClaims" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."AspNetUserClaims_Id_seq"'::"regclass");


--
-- TOC entry 2868 (class 2604 OID 39852)
-- Name: ClientClaims Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientClaims" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."ClientClaims_Id_seq"'::"regclass");


--
-- TOC entry 2869 (class 2604 OID 39853)
-- Name: ClientCorsOrigins Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientCorsOrigins" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."ClientCorsOrigins_Id_seq"'::"regclass");


--
-- TOC entry 2870 (class 2604 OID 39854)
-- Name: ClientGrantTypes Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientGrantTypes" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."ClientGrantTypes_Id_seq"'::"regclass");


--
-- TOC entry 2871 (class 2604 OID 39855)
-- Name: ClientIdPRestrictions Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientIdPRestrictions" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."ClientIdPRestrictions_Id_seq"'::"regclass");


--
-- TOC entry 2872 (class 2604 OID 39856)
-- Name: ClientPostLogoutRedirectUris Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientPostLogoutRedirectUris" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."ClientPostLogoutRedirectUris_Id_seq"'::"regclass");


--
-- TOC entry 2873 (class 2604 OID 39857)
-- Name: ClientProperties Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientProperties" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."ClientProperties_Id_seq"'::"regclass");


--
-- TOC entry 2874 (class 2604 OID 39858)
-- Name: ClientRedirectUris Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientRedirectUris" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."ClientRedirectUris_Id_seq"'::"regclass");


--
-- TOC entry 2875 (class 2604 OID 39859)
-- Name: ClientScopes Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientScopes" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."ClientScopes_Id_seq"'::"regclass");


--
-- TOC entry 2876 (class 2604 OID 39860)
-- Name: ClientSecrets Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientSecrets" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."ClientSecrets_Id_seq"'::"regclass");


--
-- TOC entry 2877 (class 2604 OID 39861)
-- Name: Clients Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."Clients" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."Clients_Id_seq"'::"regclass");


--
-- TOC entry 2878 (class 2604 OID 39862)
-- Name: IdentityClaims Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."IdentityClaims" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."IdentityClaims_Id_seq"'::"regclass");


--
-- TOC entry 2879 (class 2604 OID 39863)
-- Name: IdentityResources Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."IdentityResources" ALTER COLUMN "Id" SET DEFAULT "nextval"('"public"."IdentityResources_Id_seq"'::"regclass");


--
-- TOC entry 3104 (class 0 OID 39657)
-- Dependencies: 198
-- Data for Name: ApiClaims; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3106 (class 0 OID 39662)
-- Dependencies: 200
-- Data for Name: ApiResources; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."ApiResources" VALUES (2, true, 'dynamicapi', 'Synapse Dynamic API', NULL);


--
-- TOC entry 3108 (class 0 OID 39670)
-- Dependencies: 202
-- Data for Name: ApiScopeClaims; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."ApiScopeClaims" VALUES (3, 'name', 2);
INSERT INTO "public"."ApiScopeClaims" VALUES (4, 'name', 3);
INSERT INTO "public"."ApiScopeClaims" VALUES (5, 'email', 2);
INSERT INTO "public"."ApiScopeClaims" VALUES (6, 'email', 3);
INSERT INTO "public"."ApiScopeClaims" VALUES (9, 'given_name', 2);
INSERT INTO "public"."ApiScopeClaims" VALUES (10, 'given_name', 3);
INSERT INTO "public"."ApiScopeClaims" VALUES (2, '/synapse/adgroups', 3);
INSERT INTO "public"."ApiScopeClaims" VALUES (1, '/synapse/adgroups', 2);


--
-- TOC entry 3110 (class 0 OID 39675)
-- Dependencies: 204
-- Data for Name: ApiScopePermissions; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."ApiScopePermissions" VALUES (3, '2', '2');
INSERT INTO "public"."ApiScopePermissions" VALUES (2, '2', '3');


--
-- TOC entry 3111 (class 0 OID 39681)
-- Dependencies: 205
-- Data for Name: ApiScopes; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."ApiScopes" VALUES (2, 'dynamicapi.write', 'dynamicapi.write', 'Write Access To Dynamic API', false, false, true, 2);
INSERT INTO "public"."ApiScopes" VALUES (3, 'dynamicapi.read', 'dynamicapi.read', 'Read Access To Dynamic API', false, false, true, 2);


--
-- TOC entry 3113 (class 0 OID 39689)
-- Dependencies: 207
-- Data for Name: ApiSecrets; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3115 (class 0 OID 39697)
-- Dependencies: 209
-- Data for Name: AspNetRoleClaims; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3117 (class 0 OID 39705)
-- Dependencies: 211
-- Data for Name: AspNetRoles; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."AspNetRoles" VALUES ('2', 'DynamicApiReaders', NULL, NULL);
INSERT INTO "public"."AspNetRoles" VALUES ('1', 'DynamicApiWriters', NULL, '85c8a32e-c467-4269-af19-ad83a085197d');


--
-- TOC entry 3118 (class 0 OID 39711)
-- Dependencies: 212
-- Data for Name: AspNetUserClaims; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."AspNetUserClaims" VALUES (3, 'f58592cb-a40b-4664-bffa-9b88af1a89b0', 'family_name', 'Smith');
INSERT INTO "public"."AspNetUserClaims" VALUES (5, 'f58592cb-a40b-4664-bffa-9b88af1a89b0', 'email_verified', 'true');
INSERT INTO "public"."AspNetUserClaims" VALUES (6, 'f58592cb-a40b-4664-bffa-9b88af1a89b0', 'website', 'http://interneuron.org');
INSERT INTO "public"."AspNetUserClaims" VALUES (7, 'f58592cb-a40b-4664-bffa-9b88af1a89b0', 'address', '{ ''street_address'': ''unnamed street'', ''locality'': ''Central Town'', ''postal_code'': 577102, ''country'': ''UK'' }');
INSERT INTO "public"."AspNetUserClaims" VALUES (2, 'f58592cb-a40b-4664-bffa-9b88af1a89b0', 'given_name', 'John');
INSERT INTO "public"."AspNetUserClaims" VALUES (1, 'f58592cb-a40b-4664-bffa-9b88af1a89b0', 'name', 'John Smith');
INSERT INTO "public"."AspNetUserClaims" VALUES (4, 'f58592cb-a40b-4664-bffa-9b88af1a89b0', 'email', 'john.smith@interneuron.org');


--
-- TOC entry 3120 (class 0 OID 39719)
-- Dependencies: 214
-- Data for Name: AspNetUserLogins; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3121 (class 0 OID 39725)
-- Dependencies: 215
-- Data for Name: AspNetUserRoles; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."AspNetUserRoles" VALUES ('f58592cb-a40b-4664-bffa-9b88af1a89b0', '1');
INSERT INTO "public"."AspNetUserRoles" VALUES ('f58592cb-a40b-4664-bffa-9b88af1a89b0', '2');


--
-- TOC entry 3122 (class 0 OID 39731)
-- Dependencies: 216
-- Data for Name: AspNetUserTokens; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3123 (class 0 OID 39737)
-- Dependencies: 217
-- Data for Name: AspNetUsers; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."AspNetUsers" VALUES ('f58592cb-a40b-4664-bffa-9b88af1a89b0', 'john.smith', 'JOHN.SMITH', NULL, NULL, false, 'AQAAAAEAACcQAAAAEA4Phtw1aPpDH/2R//6Z/3VDebol00zLHXyWE9IEWmqLPLU/0PLyoxYZkXAPIEubUw==', 'cb8cb8f9-96fd-4216-bda9-76e65956b1dc', '31596859-1842-473d-8847-30d55f8080b5', NULL, false, false, NULL, true, 0);


--
-- TOC entry 3124 (class 0 OID 39743)
-- Dependencies: 218
-- Data for Name: ClientClaims; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3126 (class 0 OID 39751)
-- Dependencies: 220
-- Data for Name: ClientCorsOrigins; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."ClientCorsOrigins" VALUES (1, 'https://localhost:8085', 6);
INSERT INTO "public"."ClientCorsOrigins" VALUES (2, 'https://localhost:8085', 8);
INSERT INTO "public"."ClientCorsOrigins" VALUES (3, 'https://localhost:8085', 16);
INSERT INTO "public"."ClientCorsOrigins" VALUES (4, 'http://localhost:8084', 6);
INSERT INTO "public"."ClientCorsOrigins" VALUES (6, 'http://localhost:8084', 16);
INSERT INTO "public"."ClientCorsOrigins" VALUES (5, 'http://localhost:8084', 8);


--
-- TOC entry 3128 (class 0 OID 39756)
-- Dependencies: 222
-- Data for Name: ClientGrantTypes; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."ClientGrantTypes" VALUES (1, 'client_credentials', 1);
INSERT INTO "public"."ClientGrantTypes" VALUES (6, 'implicit', 6);
INSERT INTO "public"."ClientGrantTypes" VALUES (18, 'client_credentials', 15);
INSERT INTO "public"."ClientGrantTypes" VALUES (19, 'implicit', 16);
INSERT INTO "public"."ClientGrantTypes" VALUES (9, 'implicit', 8);


--
-- TOC entry 3130 (class 0 OID 39761)
-- Dependencies: 224
-- Data for Name: ClientIdPRestrictions; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3132 (class 0 OID 39766)
-- Dependencies: 226
-- Data for Name: ClientPostLogoutRedirectUris; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."ClientPostLogoutRedirectUris" VALUES (16, 'https://localhost:8085/studio/logout.aspx?oidccallback=true', 8);
INSERT INTO "public"."ClientPostLogoutRedirectUris" VALUES (18, 'https://localhost:8085/locatorboards/LocatorLogout.aspx?oidccallback=true', 6);
INSERT INTO "public"."ClientPostLogoutRedirectUris" VALUES (9, 'https://localhost:8085/inpatients/Logout.aspx?oidccallback=true', 6);
INSERT INTO "public"."ClientPostLogoutRedirectUris" VALUES (22, 'http://localhost:8084/locatorboards/LocatorLogout.aspx?oidccallback=true', 6);
INSERT INTO "public"."ClientPostLogoutRedirectUris" VALUES (23, 'https://localhost:8085/terminus/oidc-logout?oidccallback=true', 16);
INSERT INTO "public"."ClientPostLogoutRedirectUris" VALUES (24, 'http://localhost:8084/studio/logout.aspx?oidccallback=true', 8);
INSERT INTO "public"."ClientPostLogoutRedirectUris" VALUES (1, 'http://localhost:8084/inpatients/Logout.aspx?oidccallback=true', 6);
INSERT INTO "public"."ClientPostLogoutRedirectUris" VALUES (2, 'http://localhost:8084/terminus/oidc-logout?oidccallback=true', 16);


--
-- TOC entry 3134 (class 0 OID 39774)
-- Dependencies: 228
-- Data for Name: ClientProperties; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3136 (class 0 OID 39782)
-- Dependencies: 230
-- Data for Name: ClientRedirectUris; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."ClientRedirectUris" VALUES (38, 'http://localhost:8084/locatorboards/SilentRenew.aspx', 6);
INSERT INTO "public"."ClientRedirectUris" VALUES (39, 'http://localhost:8084/locatorboards/locatorcallback.aspx', 6);
INSERT INTO "public"."ClientRedirectUris" VALUES (41, 'https://localhost:8085/terminus/assets/silent-refresh.html', 16);
INSERT INTO "public"."ClientRedirectUris" VALUES (42, 'https://localhost:8085/terminus/oidc-callback', 16);
INSERT INTO "public"."ClientRedirectUris" VALUES (43, 'http://localhost:8084/studio/callback.aspx', 8);
INSERT INTO "public"."ClientRedirectUris" VALUES (44, 'http://localhost:8084/studio/SilentRenew.aspx', 8);
INSERT INTO "public"."ClientRedirectUris" VALUES (24, 'https://localhost:8085/studio/callback.aspx', 8);
INSERT INTO "public"."ClientRedirectUris" VALUES (26, 'https://localhost:8085/studio/SilentRenew.aspx', 8);
INSERT INTO "public"."ClientRedirectUris" VALUES (27, 'https://localhost:8085/locatorboards/locatorcallback.aspx', 6);
INSERT INTO "public"."ClientRedirectUris" VALUES (29, 'https://localhost:8085/locatorboards/SilentRenew.aspx', 6);
INSERT INTO "public"."ClientRedirectUris" VALUES (11, 'https://localhost:8085/inpatients/callback.aspx', 6);
INSERT INTO "public"."ClientRedirectUris" VALUES (13, 'https://localhost:8085/inpatients/SilentRenew.aspx', 6);
INSERT INTO "public"."ClientRedirectUris" VALUES (1, 'http://localhost:8084/terminus/assets/silent-refresh.html', 16);
INSERT INTO "public"."ClientRedirectUris" VALUES (2, 'http://localhost:8084/terminus/oidc-callback', 16);
INSERT INTO "public"."ClientRedirectUris" VALUES (3, 'http://localhost:8084/inpatients/callback.aspx', 6);
INSERT INTO "public"."ClientRedirectUris" VALUES (4, 'http://localhost:8084/inpatients/SilentRenew.aspx', 6);


--
-- TOC entry 3138 (class 0 OID 39790)
-- Dependencies: 232
-- Data for Name: ClientScopes; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."ClientScopes" VALUES (16, 'dynamicapi.read', 6);
INSERT INTO "public"."ClientScopes" VALUES (17, 'openid', 6);
INSERT INTO "public"."ClientScopes" VALUES (18, 'profile', 6);
INSERT INTO "public"."ClientScopes" VALUES (20, 'dynamicapi.read', 8);
INSERT INTO "public"."ClientScopes" VALUES (21, 'openid', 8);
INSERT INTO "public"."ClientScopes" VALUES (22, 'profile', 8);
INSERT INTO "public"."ClientScopes" VALUES (32, 'dynamicapi.read', 15);
INSERT INTO "public"."ClientScopes" VALUES (33, 'dynamicapi.read', 16);
INSERT INTO "public"."ClientScopes" VALUES (34, 'profile', 16);
INSERT INTO "public"."ClientScopes" VALUES (35, 'openid', 16);
INSERT INTO "public"."ClientScopes" VALUES (36, 'dynamicapi.read', 1);


--
-- TOC entry 3140 (class 0 OID 39795)
-- Dependencies: 234
-- Data for Name: ClientSecrets; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."ClientSecrets" VALUES (1, NULL, 'K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols=', NULL, 'SharedSecret', 1);
INSERT INTO "public"."ClientSecrets" VALUES (9, NULL, '/HttA8Vd0HKON/YS+WGNQyh2/tMHZyhjzYmgZHb54HY=', NULL, 'SharedSecret', 15);
INSERT INTO "public"."ClientSecrets" VALUES (11, NULL, 'KMIwrU1K+zDiYuDB+KLuV5K4q4o93zIqAQ9uL7KQfNQ=', NULL, 'SharedSecret', 16);


--
-- TOC entry 3142 (class 0 OID 39803)
-- Dependencies: 236
-- Data for Name: Clients; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."Clients" VALUES (1, true, 'client', 'oidc', true, NULL, NULL, NULL, NULL, true, true, false, false, false, false, NULL, true, NULL, true, false, 300, 300, 300, NULL, 2592000, 1296000, 1, false, 1, 0, true, false, false, 'client_', NULL);
INSERT INTO "public"."Clients" VALUES (8, true, 'SynapseStudio', 'oidc', false, 'Synapse Studio', NULL, NULL, NULL, false, false, false, false, false, true, NULL, true, NULL, true, false, 300, 300, 1800, NULL, 1000, 1000, 1, false, 1, 0, true, false, false, 'SS', NULL);
INSERT INTO "public"."Clients" VALUES (16, true, 'terminus-framework', 'oidc', false, 'terminus-framework', 'Terminus client', NULL, NULL, false, false, true, false, false, true, NULL, true, NULL, true, false, 300, 300, 1800, 1800, 2592000, 1296000, 1, false, 1, 0, true, false, false, '', NULL);
INSERT INTO "public"."Clients" VALUES (15, true, 'ima-backupservice', 'oidc', true, 'ima-backupservice', 'ima-backupservice', NULL, NULL, false, true, true, false, true, false, NULL, false, NULL, false, true, 300, 4200, 1800, 1800, 2592000, 1296000, 1, true, 1, 0, true, false, false, 'IBS', NULL);
INSERT INTO "public"."Clients" VALUES (6, true, 'eboards-ima', 'oidc', false, 'rnoh inpatient management applicaiton', NULL, NULL, NULL, false, false, true, false, false, true, NULL, true, NULL, true, false, 300, 300, 1800, NULL, 2592000, 1296000, 1, false, 1, 0, true, false, false, 'ima', NULL);


--
-- TOC entry 3144 (class 0 OID 39811)
-- Dependencies: 238
-- Data for Name: IdentityClaims; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."IdentityClaims" VALUES (1, 'sub', 1);
INSERT INTO "public"."IdentityClaims" VALUES (2, 'updated_at', 2);
INSERT INTO "public"."IdentityClaims" VALUES (3, 'locale', 2);
INSERT INTO "public"."IdentityClaims" VALUES (4, 'zoneinfo', 2);
INSERT INTO "public"."IdentityClaims" VALUES (5, 'birthdate', 2);
INSERT INTO "public"."IdentityClaims" VALUES (6, 'gender', 2);
INSERT INTO "public"."IdentityClaims" VALUES (7, 'website', 2);
INSERT INTO "public"."IdentityClaims" VALUES (9, 'picture', 2);
INSERT INTO "public"."IdentityClaims" VALUES (10, 'preferred_username', 2);
INSERT INTO "public"."IdentityClaims" VALUES (11, 'nickname', 2);
INSERT INTO "public"."IdentityClaims" VALUES (12, 'middle_name', 2);
INSERT INTO "public"."IdentityClaims" VALUES (14, 'family_name', 2);
INSERT INTO "public"."IdentityClaims" VALUES (15, 'name', 2);
INSERT INTO "public"."IdentityClaims" VALUES (16, 'profile', 2);
INSERT INTO "public"."IdentityClaims" VALUES (20, 'name', 6);
INSERT INTO "public"."IdentityClaims" VALUES (32, 'given_name', 2);
INSERT INTO "public"."IdentityClaims" VALUES (34, 'given_name', 12);
INSERT INTO "public"."IdentityClaims" VALUES (19, '/synapse/adgroups', 5);


--
-- TOC entry 3146 (class 0 OID 39816)
-- Dependencies: 240
-- Data for Name: IdentityResources; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."IdentityResources" VALUES (1, true, 'openid', 'Your user identifier', NULL, true, false, true);
INSERT INTO "public"."IdentityResources" VALUES (2, true, 'profile', 'User profile', 'Your user profile information (first name, last name, etc.)', false, true, true);
INSERT INTO "public"."IdentityResources" VALUES (6, true, 'name', 'name', NULL, false, true, true);
INSERT INTO "public"."IdentityResources" VALUES (12, true, 'given_name', 'given_name', 'given_name', false, false, true);
INSERT INTO "public"."IdentityResources" VALUES (5, true, '/synapse/adgroups', 'adgroups', 'All Active Directory groups for user idp:ADFS', true, true, true);


--
-- TOC entry 3148 (class 0 OID 39824)
-- Dependencies: 242
-- Data for Name: PersistedGrants; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3149 (class 0 OID 39830)
-- Dependencies: 243
-- Data for Name: UserRoles_ExternalProviders; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3150 (class 0 OID 39836)
-- Dependencies: 244
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO "public"."__EFMigrationsHistory" VALUES ('20181029115207_SynapseUsers', '2.1.4-rtm-31024');
INSERT INTO "public"."__EFMigrationsHistory" VALUES ('20181029115303_Configurations', '2.1.4-rtm-31024');
INSERT INTO "public"."__EFMigrationsHistory" VALUES ('20181029115412_PersistedGrants', '2.1.4-rtm-31024');


--
-- TOC entry 3151 (class 0 OID 39839)
-- Dependencies: 245
-- Data for Name: sislog; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3178 (class 0 OID 0)
-- Dependencies: 199
-- Name: ApiClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."ApiClaims_Id_seq"', 1, false);


--
-- TOC entry 3179 (class 0 OID 0)
-- Dependencies: 201
-- Name: ApiResources_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."ApiResources_Id_seq"', 3, true);


--
-- TOC entry 3180 (class 0 OID 0)
-- Dependencies: 203
-- Name: ApiScopeClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."ApiScopeClaims_Id_seq"', 10, true);


--
-- TOC entry 3181 (class 0 OID 0)
-- Dependencies: 206
-- Name: ApiScopes_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."ApiScopes_Id_seq"', 4, true);


--
-- TOC entry 3182 (class 0 OID 0)
-- Dependencies: 208
-- Name: ApiSecrets_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."ApiSecrets_Id_seq"', 1, false);


--
-- TOC entry 3183 (class 0 OID 0)
-- Dependencies: 210
-- Name: AspNetRoleClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."AspNetRoleClaims_Id_seq"', 1, false);


--
-- TOC entry 3184 (class 0 OID 0)
-- Dependencies: 213
-- Name: AspNetUserClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."AspNetUserClaims_Id_seq"', 440, true);


--
-- TOC entry 3185 (class 0 OID 0)
-- Dependencies: 219
-- Name: ClientClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."ClientClaims_Id_seq"', 1, false);


--
-- TOC entry 3186 (class 0 OID 0)
-- Dependencies: 221
-- Name: ClientCorsOrigins_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."ClientCorsOrigins_Id_seq"', 52, true);


--
-- TOC entry 3187 (class 0 OID 0)
-- Dependencies: 223
-- Name: ClientGrantTypes_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."ClientGrantTypes_Id_seq"', 18, true);


--
-- TOC entry 3188 (class 0 OID 0)
-- Dependencies: 225
-- Name: ClientIdPRestrictions_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."ClientIdPRestrictions_Id_seq"', 1, true);


--
-- TOC entry 3189 (class 0 OID 0)
-- Dependencies: 227
-- Name: ClientPostLogoutRedirectUris_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."ClientPostLogoutRedirectUris_Id_seq"', 56, true);


--
-- TOC entry 3190 (class 0 OID 0)
-- Dependencies: 229
-- Name: ClientProperties_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."ClientProperties_Id_seq"', 1, false);


--
-- TOC entry 3191 (class 0 OID 0)
-- Dependencies: 231
-- Name: ClientRedirectUris_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."ClientRedirectUris_Id_seq"', 75, true);


--
-- TOC entry 3192 (class 0 OID 0)
-- Dependencies: 233
-- Name: ClientScopes_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."ClientScopes_Id_seq"', 70, true);


--
-- TOC entry 3193 (class 0 OID 0)
-- Dependencies: 235
-- Name: ClientSecrets_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."ClientSecrets_Id_seq"', 42, true);


--
-- TOC entry 3194 (class 0 OID 0)
-- Dependencies: 237
-- Name: Clients_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."Clients_Id_seq"', 48, true);


--
-- TOC entry 3195 (class 0 OID 0)
-- Dependencies: 239
-- Name: IdentityClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."IdentityClaims_Id_seq"', 34, true);


--
-- TOC entry 3196 (class 0 OID 0)
-- Dependencies: 241
-- Name: IdentityResources_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"public"."IdentityResources_Id_seq"', 12, true);


--
-- TOC entry 2890 (class 2606 OID 39865)
-- Name: ApiScopePermissions ApiScopePermissions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ApiScopePermissions"
    ADD CONSTRAINT "ApiScopePermissions_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 2882 (class 2606 OID 39867)
-- Name: ApiClaims PK_ApiClaims; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ApiClaims"
    ADD CONSTRAINT "PK_ApiClaims" PRIMARY KEY ("Id");


--
-- TOC entry 2885 (class 2606 OID 39869)
-- Name: ApiResources PK_ApiResources; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ApiResources"
    ADD CONSTRAINT "PK_ApiResources" PRIMARY KEY ("Id");


--
-- TOC entry 2888 (class 2606 OID 39871)
-- Name: ApiScopeClaims PK_ApiScopeClaims; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ApiScopeClaims"
    ADD CONSTRAINT "PK_ApiScopeClaims" PRIMARY KEY ("Id");


--
-- TOC entry 2894 (class 2606 OID 39873)
-- Name: ApiScopes PK_ApiScopes; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ApiScopes"
    ADD CONSTRAINT "PK_ApiScopes" PRIMARY KEY ("Id");


--
-- TOC entry 2897 (class 2606 OID 39875)
-- Name: ApiSecrets PK_ApiSecrets; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ApiSecrets"
    ADD CONSTRAINT "PK_ApiSecrets" PRIMARY KEY ("Id");


--
-- TOC entry 2900 (class 2606 OID 39877)
-- Name: AspNetRoleClaims PK_AspNetRoleClaims; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."AspNetRoleClaims"
    ADD CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id");


--
-- TOC entry 2902 (class 2606 OID 39879)
-- Name: AspNetRoles PK_AspNetRoles; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."AspNetRoles"
    ADD CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id");


--
-- TOC entry 2906 (class 2606 OID 39881)
-- Name: AspNetUserClaims PK_AspNetUserClaims; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."AspNetUserClaims"
    ADD CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id");


--
-- TOC entry 2909 (class 2606 OID 39883)
-- Name: AspNetUserLogins PK_AspNetUserLogins; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."AspNetUserLogins"
    ADD CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey");


--
-- TOC entry 2912 (class 2606 OID 39885)
-- Name: AspNetUserRoles PK_AspNetUserRoles; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."AspNetUserRoles"
    ADD CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId");


--
-- TOC entry 2914 (class 2606 OID 39887)
-- Name: AspNetUserTokens PK_AspNetUserTokens; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."AspNetUserTokens"
    ADD CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name");


--
-- TOC entry 2917 (class 2606 OID 39889)
-- Name: AspNetUsers PK_AspNetUsers; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."AspNetUsers"
    ADD CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id");


--
-- TOC entry 2921 (class 2606 OID 39891)
-- Name: ClientClaims PK_ClientClaims; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientClaims"
    ADD CONSTRAINT "PK_ClientClaims" PRIMARY KEY ("Id");


--
-- TOC entry 2924 (class 2606 OID 39893)
-- Name: ClientCorsOrigins PK_ClientCorsOrigins; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientCorsOrigins"
    ADD CONSTRAINT "PK_ClientCorsOrigins" PRIMARY KEY ("Id");


--
-- TOC entry 2927 (class 2606 OID 39895)
-- Name: ClientGrantTypes PK_ClientGrantTypes; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientGrantTypes"
    ADD CONSTRAINT "PK_ClientGrantTypes" PRIMARY KEY ("Id");


--
-- TOC entry 2930 (class 2606 OID 39897)
-- Name: ClientIdPRestrictions PK_ClientIdPRestrictions; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientIdPRestrictions"
    ADD CONSTRAINT "PK_ClientIdPRestrictions" PRIMARY KEY ("Id");


--
-- TOC entry 2933 (class 2606 OID 39899)
-- Name: ClientPostLogoutRedirectUris PK_ClientPostLogoutRedirectUris; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientPostLogoutRedirectUris"
    ADD CONSTRAINT "PK_ClientPostLogoutRedirectUris" PRIMARY KEY ("Id");


--
-- TOC entry 2936 (class 2606 OID 39901)
-- Name: ClientProperties PK_ClientProperties; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientProperties"
    ADD CONSTRAINT "PK_ClientProperties" PRIMARY KEY ("Id");


--
-- TOC entry 2939 (class 2606 OID 39903)
-- Name: ClientRedirectUris PK_ClientRedirectUris; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientRedirectUris"
    ADD CONSTRAINT "PK_ClientRedirectUris" PRIMARY KEY ("Id");


--
-- TOC entry 2942 (class 2606 OID 39905)
-- Name: ClientScopes PK_ClientScopes; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientScopes"
    ADD CONSTRAINT "PK_ClientScopes" PRIMARY KEY ("Id");


--
-- TOC entry 2945 (class 2606 OID 39907)
-- Name: ClientSecrets PK_ClientSecrets; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientSecrets"
    ADD CONSTRAINT "PK_ClientSecrets" PRIMARY KEY ("Id");


--
-- TOC entry 2948 (class 2606 OID 39909)
-- Name: Clients PK_Clients; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."Clients"
    ADD CONSTRAINT "PK_Clients" PRIMARY KEY ("Id");


--
-- TOC entry 2951 (class 2606 OID 39911)
-- Name: IdentityClaims PK_IdentityClaims; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."IdentityClaims"
    ADD CONSTRAINT "PK_IdentityClaims" PRIMARY KEY ("Id");


--
-- TOC entry 2954 (class 2606 OID 39913)
-- Name: IdentityResources PK_IdentityResources; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."IdentityResources"
    ADD CONSTRAINT "PK_IdentityResources" PRIMARY KEY ("Id");


--
-- TOC entry 2957 (class 2606 OID 39915)
-- Name: PersistedGrants PK_PersistedGrants; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."PersistedGrants"
    ADD CONSTRAINT "PK_PersistedGrants" PRIMARY KEY ("Key");


--
-- TOC entry 2961 (class 2606 OID 39917)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 2959 (class 2606 OID 39919)
-- Name: UserRoles_ExternalProviders UserRoles_ExternalProviders_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."UserRoles_ExternalProviders"
    ADD CONSTRAINT "UserRoles_ExternalProviders_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 2915 (class 1259 OID 39920)
-- Name: EmailIndex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "EmailIndex" ON "public"."AspNetUsers" USING "btree" ("NormalizedEmail");


--
-- TOC entry 2880 (class 1259 OID 39921)
-- Name: IX_ApiClaims_ApiResourceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ApiClaims_ApiResourceId" ON "public"."ApiClaims" USING "btree" ("ApiResourceId");


--
-- TOC entry 2883 (class 1259 OID 39922)
-- Name: IX_ApiResources_Name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_ApiResources_Name" ON "public"."ApiResources" USING "btree" ("Name");


--
-- TOC entry 2886 (class 1259 OID 39923)
-- Name: IX_ApiScopeClaims_ApiScopeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ApiScopeClaims_ApiScopeId" ON "public"."ApiScopeClaims" USING "btree" ("ApiScopeId");


--
-- TOC entry 2891 (class 1259 OID 39924)
-- Name: IX_ApiScopes_ApiResourceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ApiScopes_ApiResourceId" ON "public"."ApiScopes" USING "btree" ("ApiResourceId");


--
-- TOC entry 2892 (class 1259 OID 39925)
-- Name: IX_ApiScopes_Name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_ApiScopes_Name" ON "public"."ApiScopes" USING "btree" ("Name");


--
-- TOC entry 2895 (class 1259 OID 39926)
-- Name: IX_ApiSecrets_ApiResourceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ApiSecrets_ApiResourceId" ON "public"."ApiSecrets" USING "btree" ("ApiResourceId");


--
-- TOC entry 2898 (class 1259 OID 39927)
-- Name: IX_AspNetRoleClaims_RoleId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "public"."AspNetRoleClaims" USING "btree" ("RoleId");


--
-- TOC entry 2904 (class 1259 OID 39928)
-- Name: IX_AspNetUserClaims_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "public"."AspNetUserClaims" USING "btree" ("UserId");


--
-- TOC entry 2907 (class 1259 OID 39929)
-- Name: IX_AspNetUserLogins_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "public"."AspNetUserLogins" USING "btree" ("UserId");


--
-- TOC entry 2910 (class 1259 OID 39930)
-- Name: IX_AspNetUserRoles_RoleId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "public"."AspNetUserRoles" USING "btree" ("RoleId");


--
-- TOC entry 2919 (class 1259 OID 39931)
-- Name: IX_ClientClaims_ClientId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ClientClaims_ClientId" ON "public"."ClientClaims" USING "btree" ("ClientId");


--
-- TOC entry 2922 (class 1259 OID 39932)
-- Name: IX_ClientCorsOrigins_ClientId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ClientCorsOrigins_ClientId" ON "public"."ClientCorsOrigins" USING "btree" ("ClientId");


--
-- TOC entry 2925 (class 1259 OID 39933)
-- Name: IX_ClientGrantTypes_ClientId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ClientGrantTypes_ClientId" ON "public"."ClientGrantTypes" USING "btree" ("ClientId");


--
-- TOC entry 2928 (class 1259 OID 39934)
-- Name: IX_ClientIdPRestrictions_ClientId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ClientIdPRestrictions_ClientId" ON "public"."ClientIdPRestrictions" USING "btree" ("ClientId");


--
-- TOC entry 2931 (class 1259 OID 39935)
-- Name: IX_ClientPostLogoutRedirectUris_ClientId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ClientPostLogoutRedirectUris_ClientId" ON "public"."ClientPostLogoutRedirectUris" USING "btree" ("ClientId");


--
-- TOC entry 2934 (class 1259 OID 39936)
-- Name: IX_ClientProperties_ClientId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ClientProperties_ClientId" ON "public"."ClientProperties" USING "btree" ("ClientId");


--
-- TOC entry 2937 (class 1259 OID 39937)
-- Name: IX_ClientRedirectUris_ClientId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ClientRedirectUris_ClientId" ON "public"."ClientRedirectUris" USING "btree" ("ClientId");


--
-- TOC entry 2940 (class 1259 OID 39938)
-- Name: IX_ClientScopes_ClientId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ClientScopes_ClientId" ON "public"."ClientScopes" USING "btree" ("ClientId");


--
-- TOC entry 2943 (class 1259 OID 39939)
-- Name: IX_ClientSecrets_ClientId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ClientSecrets_ClientId" ON "public"."ClientSecrets" USING "btree" ("ClientId");


--
-- TOC entry 2946 (class 1259 OID 39940)
-- Name: IX_Clients_ClientId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Clients_ClientId" ON "public"."Clients" USING "btree" ("ClientId");


--
-- TOC entry 2949 (class 1259 OID 39941)
-- Name: IX_IdentityClaims_IdentityResourceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_IdentityClaims_IdentityResourceId" ON "public"."IdentityClaims" USING "btree" ("IdentityResourceId");


--
-- TOC entry 2952 (class 1259 OID 39942)
-- Name: IX_IdentityResources_Name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_IdentityResources_Name" ON "public"."IdentityResources" USING "btree" ("Name");


--
-- TOC entry 2955 (class 1259 OID 39943)
-- Name: IX_PersistedGrants_SubjectId_ClientId_Type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_PersistedGrants_SubjectId_ClientId_Type" ON "public"."PersistedGrants" USING "btree" ("SubjectId", "ClientId", "Type");


--
-- TOC entry 2903 (class 1259 OID 39944)
-- Name: RoleNameIndex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "RoleNameIndex" ON "public"."AspNetRoles" USING "btree" ("NormalizedName");


--
-- TOC entry 2918 (class 1259 OID 39945)
-- Name: UserNameIndex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "UserNameIndex" ON "public"."AspNetUsers" USING "btree" ("NormalizedUserName");


--
-- TOC entry 2962 (class 2606 OID 39946)
-- Name: ApiClaims FK_ApiClaims_ApiResources_ApiResourceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ApiClaims"
    ADD CONSTRAINT "FK_ApiClaims_ApiResources_ApiResourceId" FOREIGN KEY ("ApiResourceId") REFERENCES "public"."ApiResources"("Id") ON DELETE CASCADE;


--
-- TOC entry 2963 (class 2606 OID 39951)
-- Name: ApiScopeClaims FK_ApiScopeClaims_ApiScopes_ApiScopeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ApiScopeClaims"
    ADD CONSTRAINT "FK_ApiScopeClaims_ApiScopes_ApiScopeId" FOREIGN KEY ("ApiScopeId") REFERENCES "public"."ApiScopes"("Id") ON DELETE CASCADE;


--
-- TOC entry 2964 (class 2606 OID 39956)
-- Name: ApiScopes FK_ApiScopes_ApiResources_ApiResourceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ApiScopes"
    ADD CONSTRAINT "FK_ApiScopes_ApiResources_ApiResourceId" FOREIGN KEY ("ApiResourceId") REFERENCES "public"."ApiResources"("Id") ON DELETE CASCADE;


--
-- TOC entry 2965 (class 2606 OID 39961)
-- Name: ApiSecrets FK_ApiSecrets_ApiResources_ApiResourceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ApiSecrets"
    ADD CONSTRAINT "FK_ApiSecrets_ApiResources_ApiResourceId" FOREIGN KEY ("ApiResourceId") REFERENCES "public"."ApiResources"("Id") ON DELETE CASCADE;


--
-- TOC entry 2966 (class 2606 OID 39966)
-- Name: AspNetRoleClaims FK_AspNetRoleClaims_AspNetRoles_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."AspNetRoleClaims"
    ADD CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "public"."AspNetRoles"("Id") ON DELETE CASCADE;


--
-- TOC entry 2967 (class 2606 OID 39971)
-- Name: AspNetUserClaims FK_AspNetUserClaims_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."AspNetUserClaims"
    ADD CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "public"."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- TOC entry 2968 (class 2606 OID 39976)
-- Name: AspNetUserLogins FK_AspNetUserLogins_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."AspNetUserLogins"
    ADD CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "public"."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- TOC entry 2969 (class 2606 OID 39981)
-- Name: AspNetUserRoles FK_AspNetUserRoles_AspNetRoles_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."AspNetUserRoles"
    ADD CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "public"."AspNetRoles"("Id") ON DELETE CASCADE;


--
-- TOC entry 2970 (class 2606 OID 39986)
-- Name: AspNetUserRoles FK_AspNetUserRoles_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."AspNetUserRoles"
    ADD CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "public"."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- TOC entry 2971 (class 2606 OID 39991)
-- Name: AspNetUserTokens FK_AspNetUserTokens_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."AspNetUserTokens"
    ADD CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "public"."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- TOC entry 2972 (class 2606 OID 39996)
-- Name: ClientClaims FK_ClientClaims_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientClaims"
    ADD CONSTRAINT "FK_ClientClaims_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "public"."Clients"("Id") ON DELETE CASCADE;


--
-- TOC entry 2973 (class 2606 OID 40001)
-- Name: ClientCorsOrigins FK_ClientCorsOrigins_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientCorsOrigins"
    ADD CONSTRAINT "FK_ClientCorsOrigins_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "public"."Clients"("Id") ON DELETE CASCADE;


--
-- TOC entry 2974 (class 2606 OID 40006)
-- Name: ClientGrantTypes FK_ClientGrantTypes_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientGrantTypes"
    ADD CONSTRAINT "FK_ClientGrantTypes_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "public"."Clients"("Id") ON DELETE CASCADE;


--
-- TOC entry 2975 (class 2606 OID 40011)
-- Name: ClientIdPRestrictions FK_ClientIdPRestrictions_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientIdPRestrictions"
    ADD CONSTRAINT "FK_ClientIdPRestrictions_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "public"."Clients"("Id") ON DELETE CASCADE;


--
-- TOC entry 2976 (class 2606 OID 40016)
-- Name: ClientPostLogoutRedirectUris FK_ClientPostLogoutRedirectUris_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientPostLogoutRedirectUris"
    ADD CONSTRAINT "FK_ClientPostLogoutRedirectUris_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "public"."Clients"("Id") ON DELETE CASCADE;


--
-- TOC entry 2977 (class 2606 OID 40021)
-- Name: ClientProperties FK_ClientProperties_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientProperties"
    ADD CONSTRAINT "FK_ClientProperties_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "public"."Clients"("Id") ON DELETE CASCADE;


--
-- TOC entry 2978 (class 2606 OID 40026)
-- Name: ClientRedirectUris FK_ClientRedirectUris_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientRedirectUris"
    ADD CONSTRAINT "FK_ClientRedirectUris_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "public"."Clients"("Id") ON DELETE CASCADE;


--
-- TOC entry 2979 (class 2606 OID 40031)
-- Name: ClientScopes FK_ClientScopes_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientScopes"
    ADD CONSTRAINT "FK_ClientScopes_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "public"."Clients"("Id") ON DELETE CASCADE;


--
-- TOC entry 2980 (class 2606 OID 40036)
-- Name: ClientSecrets FK_ClientSecrets_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."ClientSecrets"
    ADD CONSTRAINT "FK_ClientSecrets_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "public"."Clients"("Id") ON DELETE CASCADE;


--
-- TOC entry 2981 (class 2606 OID 40041)
-- Name: IdentityClaims FK_IdentityClaims_IdentityResources_IdentityResourceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "public"."IdentityClaims"
    ADD CONSTRAINT "FK_IdentityClaims_IdentityResources_IdentityResourceId" FOREIGN KEY ("IdentityResourceId") REFERENCES "public"."IdentityResources"("Id") ON DELETE CASCADE;


-- Completed on 2019-12-12 14:50:42

--
-- PostgreSQL database dump complete
--

