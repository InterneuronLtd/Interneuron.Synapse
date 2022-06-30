--
-- PostgreSQL database dump
--

-- Dumped from database version 13.4
-- Dumped by pg_dump version 14.1 (Ubuntu 14.1-1.pgdg18.04+1)

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

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: ApiClaims; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."ApiClaims" (
    "Id" integer NOT NULL,
    "Type" character varying(200) NOT NULL,
    "ApiResourceId" integer NOT NULL
);


ALTER TABLE public."ApiClaims" OWNER TO owner_name;

--
-- Name: ApiClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."ApiClaims_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ApiClaims_Id_seq" OWNER TO owner_name;

--
-- Name: ApiClaims_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."ApiClaims_Id_seq" OWNED BY public."ApiClaims"."Id";


--
-- Name: ApiResources; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."ApiResources" (
    "Id" integer NOT NULL,
    "Enabled" boolean NOT NULL,
    "Name" character varying(200) NOT NULL,
    "DisplayName" character varying(200),
    "Description" character varying(1000)
);


ALTER TABLE public."ApiResources" OWNER TO owner_name;

--
-- Name: ApiResources_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."ApiResources_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ApiResources_Id_seq" OWNER TO owner_name;

--
-- Name: ApiResources_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."ApiResources_Id_seq" OWNED BY public."ApiResources"."Id";


--
-- Name: ApiScopeClaims; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."ApiScopeClaims" (
    "Id" integer NOT NULL,
    "Type" character varying(200) NOT NULL,
    "ApiScopeId" integer NOT NULL
);


ALTER TABLE public."ApiScopeClaims" OWNER TO owner_name;

--
-- Name: ApiScopeClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."ApiScopeClaims_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ApiScopeClaims_Id_seq" OWNER TO owner_name;

--
-- Name: ApiScopeClaims_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."ApiScopeClaims_Id_seq" OWNED BY public."ApiScopeClaims"."Id";


--
-- Name: ApiScopePermissions; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."ApiScopePermissions" (
    "ApiScopeId" integer NOT NULL,
    "RoleId" text NOT NULL,
    "Id" text NOT NULL
);


ALTER TABLE public."ApiScopePermissions" OWNER TO owner_name;

--
-- Name: ApiScopes; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."ApiScopes" (
    "Id" integer NOT NULL,
    "Name" character varying(200) NOT NULL,
    "DisplayName" character varying(200),
    "Description" character varying(1000),
    "Required" boolean NOT NULL,
    "Emphasize" boolean NOT NULL,
    "ShowInDiscoveryDocument" boolean NOT NULL,
    "ApiResourceId" integer NOT NULL
);


ALTER TABLE public."ApiScopes" OWNER TO owner_name;

--
-- Name: ApiScopes_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."ApiScopes_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ApiScopes_Id_seq" OWNER TO owner_name;

--
-- Name: ApiScopes_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."ApiScopes_Id_seq" OWNED BY public."ApiScopes"."Id";


--
-- Name: ApiSecrets; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."ApiSecrets" (
    "Id" integer NOT NULL,
    "Description" character varying(1000),
    "Value" character varying(2000),
    "Expiration" timestamp without time zone,
    "Type" character varying(250),
    "ApiResourceId" integer NOT NULL
);


ALTER TABLE public."ApiSecrets" OWNER TO owner_name;

--
-- Name: ApiSecrets_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."ApiSecrets_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ApiSecrets_Id_seq" OWNER TO owner_name;

--
-- Name: ApiSecrets_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."ApiSecrets_Id_seq" OWNED BY public."ApiSecrets"."Id";


--
-- Name: AspNetRoleClaims; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."AspNetRoleClaims" (
    "Id" integer NOT NULL,
    "RoleId" text NOT NULL,
    "ClaimType" text,
    "ClaimValue" text
);


ALTER TABLE public."AspNetRoleClaims" OWNER TO owner_name;

--
-- Name: AspNetRoleClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."AspNetRoleClaims_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."AspNetRoleClaims_Id_seq" OWNER TO owner_name;

--
-- Name: AspNetRoleClaims_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."AspNetRoleClaims_Id_seq" OWNED BY public."AspNetRoleClaims"."Id";


--
-- Name: AspNetRoles; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."AspNetRoles" (
    "Id" text NOT NULL,
    "Name" character varying(256),
    "NormalizedName" character varying(256),
    "ConcurrencyStamp" text
);


ALTER TABLE public."AspNetRoles" OWNER TO owner_name;

--
-- Name: AspNetUserClaims; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."AspNetUserClaims" (
    "Id" integer NOT NULL,
    "UserId" text NOT NULL,
    "ClaimType" text,
    "ClaimValue" text
);


ALTER TABLE public."AspNetUserClaims" OWNER TO owner_name;

--
-- Name: AspNetUserClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."AspNetUserClaims_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."AspNetUserClaims_Id_seq" OWNER TO owner_name;

--
-- Name: AspNetUserClaims_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."AspNetUserClaims_Id_seq" OWNED BY public."AspNetUserClaims"."Id";


--
-- Name: AspNetUserLogins; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."AspNetUserLogins" (
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text,
    "UserId" text NOT NULL
);


ALTER TABLE public."AspNetUserLogins" OWNER TO owner_name;

--
-- Name: AspNetUserRoles; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."AspNetUserRoles" (
    "UserId" text NOT NULL,
    "RoleId" text NOT NULL
);


ALTER TABLE public."AspNetUserRoles" OWNER TO owner_name;

--
-- Name: AspNetUserTokens; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."AspNetUserTokens" (
    "UserId" text NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text
);


ALTER TABLE public."AspNetUserTokens" OWNER TO owner_name;

--
-- Name: AspNetUsers; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."AspNetUsers" (
    "Id" text NOT NULL,
    "UserName" character varying(256),
    "NormalizedUserName" character varying(256),
    "Email" character varying(256),
    "NormalizedEmail" character varying(256),
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text,
    "SecurityStamp" text,
    "ConcurrencyStamp" text,
    "PhoneNumber" text,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL
);


ALTER TABLE public."AspNetUsers" OWNER TO owner_name;

--
-- Name: ClientClaims; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."ClientClaims" (
    "Id" integer NOT NULL,
    "Type" character varying(250) NOT NULL,
    "Value" character varying(250) NOT NULL,
    "ClientId" integer NOT NULL
);


ALTER TABLE public."ClientClaims" OWNER TO owner_name;

--
-- Name: ClientClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."ClientClaims_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ClientClaims_Id_seq" OWNER TO owner_name;

--
-- Name: ClientClaims_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."ClientClaims_Id_seq" OWNED BY public."ClientClaims"."Id";


--
-- Name: ClientCorsOrigins; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."ClientCorsOrigins" (
    "Id" integer NOT NULL,
    "Origin" character varying(150) NOT NULL,
    "ClientId" integer NOT NULL
);


ALTER TABLE public."ClientCorsOrigins" OWNER TO owner_name;

--
-- Name: ClientCorsOrigins_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."ClientCorsOrigins_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ClientCorsOrigins_Id_seq" OWNER TO owner_name;

--
-- Name: ClientCorsOrigins_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."ClientCorsOrigins_Id_seq" OWNED BY public."ClientCorsOrigins"."Id";


--
-- Name: ClientGrantTypes; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."ClientGrantTypes" (
    "Id" integer NOT NULL,
    "GrantType" character varying(250) NOT NULL,
    "ClientId" integer NOT NULL
);


ALTER TABLE public."ClientGrantTypes" OWNER TO owner_name;

--
-- Name: ClientGrantTypes_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."ClientGrantTypes_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ClientGrantTypes_Id_seq" OWNER TO owner_name;

--
-- Name: ClientGrantTypes_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."ClientGrantTypes_Id_seq" OWNED BY public."ClientGrantTypes"."Id";


--
-- Name: ClientIdPRestrictions; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."ClientIdPRestrictions" (
    "Id" integer NOT NULL,
    "Provider" character varying(200) NOT NULL,
    "ClientId" integer NOT NULL
);


ALTER TABLE public."ClientIdPRestrictions" OWNER TO owner_name;

--
-- Name: ClientIdPRestrictions_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."ClientIdPRestrictions_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ClientIdPRestrictions_Id_seq" OWNER TO owner_name;

--
-- Name: ClientIdPRestrictions_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."ClientIdPRestrictions_Id_seq" OWNED BY public."ClientIdPRestrictions"."Id";


--
-- Name: ClientPostLogoutRedirectUris; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."ClientPostLogoutRedirectUris" (
    "Id" integer NOT NULL,
    "PostLogoutRedirectUri" character varying(2000) NOT NULL,
    "ClientId" integer NOT NULL
);


ALTER TABLE public."ClientPostLogoutRedirectUris" OWNER TO owner_name;

--
-- Name: ClientPostLogoutRedirectUris_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."ClientPostLogoutRedirectUris_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ClientPostLogoutRedirectUris_Id_seq" OWNER TO owner_name;

--
-- Name: ClientPostLogoutRedirectUris_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."ClientPostLogoutRedirectUris_Id_seq" OWNED BY public."ClientPostLogoutRedirectUris"."Id";


--
-- Name: ClientProperties; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."ClientProperties" (
    "Id" integer NOT NULL,
    "Key" character varying(250) NOT NULL,
    "Value" character varying(2000) NOT NULL,
    "ClientId" integer NOT NULL
);


ALTER TABLE public."ClientProperties" OWNER TO owner_name;

--
-- Name: ClientProperties_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."ClientProperties_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ClientProperties_Id_seq" OWNER TO owner_name;

--
-- Name: ClientProperties_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."ClientProperties_Id_seq" OWNED BY public."ClientProperties"."Id";


--
-- Name: ClientRedirectUris; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."ClientRedirectUris" (
    "Id" integer NOT NULL,
    "RedirectUri" character varying(2000) NOT NULL,
    "ClientId" integer NOT NULL
);


ALTER TABLE public."ClientRedirectUris" OWNER TO owner_name;

--
-- Name: ClientRedirectUris_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."ClientRedirectUris_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ClientRedirectUris_Id_seq" OWNER TO owner_name;

--
-- Name: ClientRedirectUris_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."ClientRedirectUris_Id_seq" OWNED BY public."ClientRedirectUris"."Id";


--
-- Name: ClientScopes; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."ClientScopes" (
    "Id" integer NOT NULL,
    "Scope" character varying(200) NOT NULL,
    "ClientId" integer NOT NULL
);


ALTER TABLE public."ClientScopes" OWNER TO owner_name;

--
-- Name: ClientScopes_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."ClientScopes_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ClientScopes_Id_seq" OWNER TO owner_name;

--
-- Name: ClientScopes_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."ClientScopes_Id_seq" OWNED BY public."ClientScopes"."Id";


--
-- Name: ClientSecrets; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."ClientSecrets" (
    "Id" integer NOT NULL,
    "Description" character varying(2000),
    "Value" character varying(2000) NOT NULL,
    "Expiration" timestamp without time zone,
    "Type" character varying(250),
    "ClientId" integer NOT NULL
);


ALTER TABLE public."ClientSecrets" OWNER TO owner_name;

--
-- Name: ClientSecrets_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."ClientSecrets_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ClientSecrets_Id_seq" OWNER TO owner_name;

--
-- Name: ClientSecrets_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."ClientSecrets_Id_seq" OWNED BY public."ClientSecrets"."Id";


--
-- Name: Clients; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."Clients" (
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


ALTER TABLE public."Clients" OWNER TO owner_name;

--
-- Name: Clients_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."Clients_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Clients_Id_seq" OWNER TO owner_name;

--
-- Name: Clients_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."Clients_Id_seq" OWNED BY public."Clients"."Id";


--
-- Name: IdentityClaims; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."IdentityClaims" (
    "Id" integer NOT NULL,
    "Type" character varying(200) NOT NULL,
    "IdentityResourceId" integer NOT NULL
);


ALTER TABLE public."IdentityClaims" OWNER TO owner_name;

--
-- Name: IdentityClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."IdentityClaims_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."IdentityClaims_Id_seq" OWNER TO owner_name;

--
-- Name: IdentityClaims_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."IdentityClaims_Id_seq" OWNED BY public."IdentityClaims"."Id";


--
-- Name: IdentityResources; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."IdentityResources" (
    "Id" integer NOT NULL,
    "Enabled" boolean NOT NULL,
    "Name" character varying(200) NOT NULL,
    "DisplayName" character varying(200),
    "Description" character varying(1000),
    "Required" boolean NOT NULL,
    "Emphasize" boolean NOT NULL,
    "ShowInDiscoveryDocument" boolean NOT NULL
);


ALTER TABLE public."IdentityResources" OWNER TO owner_name;

--
-- Name: IdentityResources_Id_seq; Type: SEQUENCE; Schema: public; Owner: owner_name
--

CREATE SEQUENCE public."IdentityResources_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."IdentityResources_Id_seq" OWNER TO owner_name;

--
-- Name: IdentityResources_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: owner_name
--

ALTER SEQUENCE public."IdentityResources_Id_seq" OWNED BY public."IdentityResources"."Id";


--
-- Name: PersistedGrants; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."PersistedGrants" (
    "Key" character varying(200) NOT NULL,
    "Type" character varying(50) NOT NULL,
    "SubjectId" character varying(200),
    "ClientId" character varying(200) NOT NULL,
    "CreationTime" timestamp without time zone NOT NULL,
    "Expiration" timestamp without time zone,
    "Data" character varying(50000) NOT NULL
);


ALTER TABLE public."PersistedGrants" OWNER TO owner_name;

--
-- Name: UserRoles_ExternalProviders; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."UserRoles_ExternalProviders" (
    "RoleId" text NOT NULL,
    "ExternalSubjectId" text NOT NULL,
    "Id" text NOT NULL,
    "Idp" text
);


ALTER TABLE public."UserRoles_ExternalProviders" OWNER TO owner_name;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO owner_name;

--
-- Name: sislog; Type: TABLE; Schema: public; Owner: owner_name
--

CREATE TABLE public.sislog (
    message text,
    message_template text,
    level integer,
    "timestamp" timestamp without time zone,
    exception text,
    log_event jsonb
);


ALTER TABLE public.sislog OWNER TO owner_name;

--
-- Name: ApiClaims Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ApiClaims" ALTER COLUMN "Id" SET DEFAULT nextval('public."ApiClaims_Id_seq"'::regclass);


--
-- Name: ApiResources Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ApiResources" ALTER COLUMN "Id" SET DEFAULT nextval('public."ApiResources_Id_seq"'::regclass);


--
-- Name: ApiScopeClaims Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ApiScopeClaims" ALTER COLUMN "Id" SET DEFAULT nextval('public."ApiScopeClaims_Id_seq"'::regclass);


--
-- Name: ApiScopes Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ApiScopes" ALTER COLUMN "Id" SET DEFAULT nextval('public."ApiScopes_Id_seq"'::regclass);


--
-- Name: ApiSecrets Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ApiSecrets" ALTER COLUMN "Id" SET DEFAULT nextval('public."ApiSecrets_Id_seq"'::regclass);


--
-- Name: AspNetRoleClaims Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."AspNetRoleClaims" ALTER COLUMN "Id" SET DEFAULT nextval('public."AspNetRoleClaims_Id_seq"'::regclass);


--
-- Name: AspNetUserClaims Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."AspNetUserClaims" ALTER COLUMN "Id" SET DEFAULT nextval('public."AspNetUserClaims_Id_seq"'::regclass);


--
-- Name: ClientClaims Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientClaims" ALTER COLUMN "Id" SET DEFAULT nextval('public."ClientClaims_Id_seq"'::regclass);


--
-- Name: ClientCorsOrigins Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientCorsOrigins" ALTER COLUMN "Id" SET DEFAULT nextval('public."ClientCorsOrigins_Id_seq"'::regclass);


--
-- Name: ClientGrantTypes Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientGrantTypes" ALTER COLUMN "Id" SET DEFAULT nextval('public."ClientGrantTypes_Id_seq"'::regclass);


--
-- Name: ClientIdPRestrictions Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientIdPRestrictions" ALTER COLUMN "Id" SET DEFAULT nextval('public."ClientIdPRestrictions_Id_seq"'::regclass);


--
-- Name: ClientPostLogoutRedirectUris Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientPostLogoutRedirectUris" ALTER COLUMN "Id" SET DEFAULT nextval('public."ClientPostLogoutRedirectUris_Id_seq"'::regclass);


--
-- Name: ClientProperties Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientProperties" ALTER COLUMN "Id" SET DEFAULT nextval('public."ClientProperties_Id_seq"'::regclass);


--
-- Name: ClientRedirectUris Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientRedirectUris" ALTER COLUMN "Id" SET DEFAULT nextval('public."ClientRedirectUris_Id_seq"'::regclass);


--
-- Name: ClientScopes Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientScopes" ALTER COLUMN "Id" SET DEFAULT nextval('public."ClientScopes_Id_seq"'::regclass);


--
-- Name: ClientSecrets Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientSecrets" ALTER COLUMN "Id" SET DEFAULT nextval('public."ClientSecrets_Id_seq"'::regclass);


--
-- Name: Clients Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."Clients" ALTER COLUMN "Id" SET DEFAULT nextval('public."Clients_Id_seq"'::regclass);


--
-- Name: IdentityClaims Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."IdentityClaims" ALTER COLUMN "Id" SET DEFAULT nextval('public."IdentityClaims_Id_seq"'::regclass);


--
-- Name: IdentityResources Id; Type: DEFAULT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."IdentityResources" ALTER COLUMN "Id" SET DEFAULT nextval('public."IdentityResources_Id_seq"'::regclass);


--
-- Name: ApiScopePermissions ApiScopePermissions_pkey; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ApiScopePermissions"
    ADD CONSTRAINT "ApiScopePermissions_pkey" PRIMARY KEY ("Id");


--
-- Name: ApiClaims PK_ApiClaims; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ApiClaims"
    ADD CONSTRAINT "PK_ApiClaims" PRIMARY KEY ("Id");


--
-- Name: ApiResources PK_ApiResources; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ApiResources"
    ADD CONSTRAINT "PK_ApiResources" PRIMARY KEY ("Id");


--
-- Name: ApiScopeClaims PK_ApiScopeClaims; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ApiScopeClaims"
    ADD CONSTRAINT "PK_ApiScopeClaims" PRIMARY KEY ("Id");


--
-- Name: ApiScopes PK_ApiScopes; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ApiScopes"
    ADD CONSTRAINT "PK_ApiScopes" PRIMARY KEY ("Id");


--
-- Name: ApiSecrets PK_ApiSecrets; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ApiSecrets"
    ADD CONSTRAINT "PK_ApiSecrets" PRIMARY KEY ("Id");


--
-- Name: AspNetRoleClaims PK_AspNetRoleClaims; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."AspNetRoleClaims"
    ADD CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id");


--
-- Name: AspNetRoles PK_AspNetRoles; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."AspNetRoles"
    ADD CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id");


--
-- Name: AspNetUserClaims PK_AspNetUserClaims; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."AspNetUserClaims"
    ADD CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id");


--
-- Name: AspNetUserLogins PK_AspNetUserLogins; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."AspNetUserLogins"
    ADD CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey");


--
-- Name: AspNetUserRoles PK_AspNetUserRoles; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId");


--
-- Name: AspNetUserTokens PK_AspNetUserTokens; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."AspNetUserTokens"
    ADD CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name");


--
-- Name: AspNetUsers PK_AspNetUsers; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."AspNetUsers"
    ADD CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id");


--
-- Name: ClientClaims PK_ClientClaims; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientClaims"
    ADD CONSTRAINT "PK_ClientClaims" PRIMARY KEY ("Id");


--
-- Name: ClientCorsOrigins PK_ClientCorsOrigins; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientCorsOrigins"
    ADD CONSTRAINT "PK_ClientCorsOrigins" PRIMARY KEY ("Id");


--
-- Name: ClientGrantTypes PK_ClientGrantTypes; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientGrantTypes"
    ADD CONSTRAINT "PK_ClientGrantTypes" PRIMARY KEY ("Id");


--
-- Name: ClientIdPRestrictions PK_ClientIdPRestrictions; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientIdPRestrictions"
    ADD CONSTRAINT "PK_ClientIdPRestrictions" PRIMARY KEY ("Id");


--
-- Name: ClientPostLogoutRedirectUris PK_ClientPostLogoutRedirectUris; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientPostLogoutRedirectUris"
    ADD CONSTRAINT "PK_ClientPostLogoutRedirectUris" PRIMARY KEY ("Id");


--
-- Name: ClientProperties PK_ClientProperties; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientProperties"
    ADD CONSTRAINT "PK_ClientProperties" PRIMARY KEY ("Id");


--
-- Name: ClientRedirectUris PK_ClientRedirectUris; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientRedirectUris"
    ADD CONSTRAINT "PK_ClientRedirectUris" PRIMARY KEY ("Id");


--
-- Name: ClientScopes PK_ClientScopes; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientScopes"
    ADD CONSTRAINT "PK_ClientScopes" PRIMARY KEY ("Id");


--
-- Name: ClientSecrets PK_ClientSecrets; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientSecrets"
    ADD CONSTRAINT "PK_ClientSecrets" PRIMARY KEY ("Id");


--
-- Name: Clients PK_Clients; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."Clients"
    ADD CONSTRAINT "PK_Clients" PRIMARY KEY ("Id");


--
-- Name: IdentityClaims PK_IdentityClaims; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."IdentityClaims"
    ADD CONSTRAINT "PK_IdentityClaims" PRIMARY KEY ("Id");


--
-- Name: IdentityResources PK_IdentityResources; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."IdentityResources"
    ADD CONSTRAINT "PK_IdentityResources" PRIMARY KEY ("Id");


--
-- Name: PersistedGrants PK_PersistedGrants; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."PersistedGrants"
    ADD CONSTRAINT "PK_PersistedGrants" PRIMARY KEY ("Key");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: UserRoles_ExternalProviders UserRoles_ExternalProviders_pkey; Type: CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."UserRoles_ExternalProviders"
    ADD CONSTRAINT "UserRoles_ExternalProviders_pkey" PRIMARY KEY ("Id");


--
-- Name: EmailIndex; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "EmailIndex" ON public."AspNetUsers" USING btree ("NormalizedEmail");


--
-- Name: IX_ApiClaims_ApiResourceId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_ApiClaims_ApiResourceId" ON public."ApiClaims" USING btree ("ApiResourceId");


--
-- Name: IX_ApiResources_Name; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE UNIQUE INDEX "IX_ApiResources_Name" ON public."ApiResources" USING btree ("Name");


--
-- Name: IX_ApiScopeClaims_ApiScopeId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_ApiScopeClaims_ApiScopeId" ON public."ApiScopeClaims" USING btree ("ApiScopeId");


--
-- Name: IX_ApiScopes_ApiResourceId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_ApiScopes_ApiResourceId" ON public."ApiScopes" USING btree ("ApiResourceId");


--
-- Name: IX_ApiScopes_Name; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE UNIQUE INDEX "IX_ApiScopes_Name" ON public."ApiScopes" USING btree ("Name");


--
-- Name: IX_ApiSecrets_ApiResourceId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_ApiSecrets_ApiResourceId" ON public."ApiSecrets" USING btree ("ApiResourceId");


--
-- Name: IX_AspNetRoleClaims_RoleId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON public."AspNetRoleClaims" USING btree ("RoleId");


--
-- Name: IX_AspNetUserClaims_UserId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_AspNetUserClaims_UserId" ON public."AspNetUserClaims" USING btree ("UserId");


--
-- Name: IX_AspNetUserLogins_UserId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_AspNetUserLogins_UserId" ON public."AspNetUserLogins" USING btree ("UserId");


--
-- Name: IX_AspNetUserRoles_RoleId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON public."AspNetUserRoles" USING btree ("RoleId");


--
-- Name: IX_ClientClaims_ClientId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_ClientClaims_ClientId" ON public."ClientClaims" USING btree ("ClientId");


--
-- Name: IX_ClientCorsOrigins_ClientId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_ClientCorsOrigins_ClientId" ON public."ClientCorsOrigins" USING btree ("ClientId");


--
-- Name: IX_ClientGrantTypes_ClientId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_ClientGrantTypes_ClientId" ON public."ClientGrantTypes" USING btree ("ClientId");


--
-- Name: IX_ClientIdPRestrictions_ClientId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_ClientIdPRestrictions_ClientId" ON public."ClientIdPRestrictions" USING btree ("ClientId");


--
-- Name: IX_ClientPostLogoutRedirectUris_ClientId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_ClientPostLogoutRedirectUris_ClientId" ON public."ClientPostLogoutRedirectUris" USING btree ("ClientId");


--
-- Name: IX_ClientProperties_ClientId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_ClientProperties_ClientId" ON public."ClientProperties" USING btree ("ClientId");


--
-- Name: IX_ClientRedirectUris_ClientId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_ClientRedirectUris_ClientId" ON public."ClientRedirectUris" USING btree ("ClientId");


--
-- Name: IX_ClientScopes_ClientId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_ClientScopes_ClientId" ON public."ClientScopes" USING btree ("ClientId");


--
-- Name: IX_ClientSecrets_ClientId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_ClientSecrets_ClientId" ON public."ClientSecrets" USING btree ("ClientId");


--
-- Name: IX_Clients_ClientId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE UNIQUE INDEX "IX_Clients_ClientId" ON public."Clients" USING btree ("ClientId");


--
-- Name: IX_IdentityClaims_IdentityResourceId; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_IdentityClaims_IdentityResourceId" ON public."IdentityClaims" USING btree ("IdentityResourceId");


--
-- Name: IX_IdentityResources_Name; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE UNIQUE INDEX "IX_IdentityResources_Name" ON public."IdentityResources" USING btree ("Name");


--
-- Name: IX_PersistedGrants_SubjectId_ClientId_Type; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE INDEX "IX_PersistedGrants_SubjectId_ClientId_Type" ON public."PersistedGrants" USING btree ("SubjectId", "ClientId", "Type");


--
-- Name: RoleNameIndex; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE UNIQUE INDEX "RoleNameIndex" ON public."AspNetRoles" USING btree ("NormalizedName");


--
-- Name: UserNameIndex; Type: INDEX; Schema: public; Owner: owner_name
--

CREATE UNIQUE INDEX "UserNameIndex" ON public."AspNetUsers" USING btree ("NormalizedUserName");


--
-- Name: ApiClaims FK_ApiClaims_ApiResources_ApiResourceId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ApiClaims"
    ADD CONSTRAINT "FK_ApiClaims_ApiResources_ApiResourceId" FOREIGN KEY ("ApiResourceId") REFERENCES public."ApiResources"("Id") ON DELETE CASCADE;


--
-- Name: ApiScopeClaims FK_ApiScopeClaims_ApiScopes_ApiScopeId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ApiScopeClaims"
    ADD CONSTRAINT "FK_ApiScopeClaims_ApiScopes_ApiScopeId" FOREIGN KEY ("ApiScopeId") REFERENCES public."ApiScopes"("Id") ON DELETE CASCADE;


--
-- Name: ApiScopes FK_ApiScopes_ApiResources_ApiResourceId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ApiScopes"
    ADD CONSTRAINT "FK_ApiScopes_ApiResources_ApiResourceId" FOREIGN KEY ("ApiResourceId") REFERENCES public."ApiResources"("Id") ON DELETE CASCADE;


--
-- Name: ApiSecrets FK_ApiSecrets_ApiResources_ApiResourceId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ApiSecrets"
    ADD CONSTRAINT "FK_ApiSecrets_ApiResources_ApiResourceId" FOREIGN KEY ("ApiResourceId") REFERENCES public."ApiResources"("Id") ON DELETE CASCADE;


--
-- Name: AspNetRoleClaims FK_AspNetRoleClaims_AspNetRoles_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."AspNetRoleClaims"
    ADD CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."AspNetRoles"("Id") ON DELETE CASCADE;


--
-- Name: AspNetUserClaims FK_AspNetUserClaims_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."AspNetUserClaims"
    ADD CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: AspNetUserLogins FK_AspNetUserLogins_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."AspNetUserLogins"
    ADD CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: AspNetUserRoles FK_AspNetUserRoles_AspNetRoles_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."AspNetRoles"("Id") ON DELETE CASCADE;


--
-- Name: AspNetUserRoles FK_AspNetUserRoles_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: AspNetUserTokens FK_AspNetUserTokens_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."AspNetUserTokens"
    ADD CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: ClientClaims FK_ClientClaims_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientClaims"
    ADD CONSTRAINT "FK_ClientClaims_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES public."Clients"("Id") ON DELETE CASCADE;


--
-- Name: ClientCorsOrigins FK_ClientCorsOrigins_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientCorsOrigins"
    ADD CONSTRAINT "FK_ClientCorsOrigins_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES public."Clients"("Id") ON DELETE CASCADE;


--
-- Name: ClientGrantTypes FK_ClientGrantTypes_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientGrantTypes"
    ADD CONSTRAINT "FK_ClientGrantTypes_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES public."Clients"("Id") ON DELETE CASCADE;


--
-- Name: ClientIdPRestrictions FK_ClientIdPRestrictions_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientIdPRestrictions"
    ADD CONSTRAINT "FK_ClientIdPRestrictions_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES public."Clients"("Id") ON DELETE CASCADE;


--
-- Name: ClientPostLogoutRedirectUris FK_ClientPostLogoutRedirectUris_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientPostLogoutRedirectUris"
    ADD CONSTRAINT "FK_ClientPostLogoutRedirectUris_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES public."Clients"("Id") ON DELETE CASCADE;


--
-- Name: ClientProperties FK_ClientProperties_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientProperties"
    ADD CONSTRAINT "FK_ClientProperties_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES public."Clients"("Id") ON DELETE CASCADE;


--
-- Name: ClientRedirectUris FK_ClientRedirectUris_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientRedirectUris"
    ADD CONSTRAINT "FK_ClientRedirectUris_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES public."Clients"("Id") ON DELETE CASCADE;


--
-- Name: ClientScopes FK_ClientScopes_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientScopes"
    ADD CONSTRAINT "FK_ClientScopes_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES public."Clients"("Id") ON DELETE CASCADE;


--
-- Name: ClientSecrets FK_ClientSecrets_Clients_ClientId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."ClientSecrets"
    ADD CONSTRAINT "FK_ClientSecrets_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES public."Clients"("Id") ON DELETE CASCADE;


--
-- Name: IdentityClaims FK_IdentityClaims_IdentityResources_IdentityResourceId; Type: FK CONSTRAINT; Schema: public; Owner: owner_name
--

ALTER TABLE ONLY public."IdentityClaims"
    ADD CONSTRAINT "FK_IdentityClaims_IdentityResources_IdentityResourceId" FOREIGN KEY ("IdentityResourceId") REFERENCES public."IdentityResources"("Id") ON DELETE CASCADE;


--
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: owner_name
--

REVOKE ALL ON SCHEMA public FROM rdsadmin;
REVOKE ALL ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO owner_name;
GRANT ALL ON SCHEMA public TO PUBLIC;


--
-- PostgreSQL database dump complete
--

