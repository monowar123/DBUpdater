CREATE OR REPLACE VIEW v_vlakobjecten AS 
 SELECT hitbgt.cadid,
    hitbgt.bgtgvt,
    hitbgt.crtdate,
    hitbgt.bgtid,
    hitbgt.bgttype,
    hitbgt.tmain,
    hitbgt.tsub,
    hitbgt.fmain,
    hitbgt.fsub,
    0::double precision AS float8,
    hitbgt.level,
    hitbgt.bagid,
    hitbgt.geotxt,
    hitbgt.geotxtlin,
    hitbgt.lastupdate,
    hitbgt.status,
    hitbgt.plusstatus,
    hitbgt.inonderzoek,
    hitbgt.extra1,
    hitbgt.extra2,
    hitbgt.extra3,
    hitbgt.extra4,
    hitbgt.the_geom,
    fn_hitgv_tooltip(hitbgt.bgttype, hitbgt.bgtid, hitbgt.bgtgvt, hitbgt.rfunc, hitbgt.otype, hitbgt.bagid, hitbgt.talud, hitbgt.level, hitbgt.extra1, bot.typename) AS tooltiptext,
    'hitbgt'::text AS hittype,
    hitbgt.remark,
    hitbgt.mapnr,
    hitbgt.talud,
    hitbgt.extra5,
    ''::character varying AS geotype
   FROM hitbgt
     LEFT JOIN bgtobjecttypes bot ON hitbgt.bgttype::text = bot.bgttype::text
  WHERE COALESCE(NULLIF(hitbgt.level::text, ''::text), '0'::text) <> '0'::text
UNION
 SELECT hitbgtoth.cadid,
    hitbgtoth.bgtgvt,
    hitbgtoth.crtdate,
    hitbgtoth.bgtid,
    hitbgtoth.bgttype,
    hitbgtoth.tmain,
    hitbgtoth.tsub,
    ''::character varying AS fmain,
    ''::character varying AS fsub,
    hitbgtoth.rotation AS float8,
    hitbgtoth.level,
    ''::character varying AS bagid,
    hitbgtoth.geotxt,
    hitbgtoth.geotxtlin,
    hitbgtoth.lastupdate,
    hitbgtoth.status,
    hitbgtoth.plusstatus,
    hitbgtoth.inonderzoek,
    hitbgtoth.extra1,
    hitbgtoth.extra2,
    hitbgtoth.extra3,
    hitbgtoth.extra4,
    hitbgtoth.the_geom,
    fn_hitothgv_tooltip(hitbgtoth.bgttype, hitbgtoth.bgtid, hitbgtoth.bgtgvt, ''::character varying, hitbgtoth.otype, ''::character varying, ''::character varying, hitbgtoth.level, bot.typename) AS tooltiptext,
    'hitbgtoth'::text AS hittype,
    hitbgtoth.remark,
    hitbgtoth.mapnr,
    ''::character varying AS talud,
    hitbgtoth.extra5,
    hitbgtoth.geotype
   FROM hitbgtoth
     LEFT JOIN bgtobjecttypes bot ON hitbgtoth.bgttype::text = bot.bgttype::text
  WHERE lower("substring"(hitbgtoth.geotype::text, 1, 1)) = 'v'::text;

ALTER TABLE v_vlakobjecten
  OWNER TO "AOXX";