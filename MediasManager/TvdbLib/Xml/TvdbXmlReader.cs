﻿/*
 *   TvdbLib: A library to retrieve information and media from http://thetvdb.com
 * 
 *   Copyright (C) 2008  Benjamin Gmeiner
 * 
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TvdbLib.Data;
using TvdbLib.Data.Banner;
using System.Diagnostics;
using System.Drawing;

namespace TvdbLib.Xml
{
  /// <summary>
  /// Class for parsing the xml info from thetvdb
  /// </summary>
  internal class TvdbXmlReader
  {
    /// <summary>
    /// Base constructor for a TvdbXmlReader class
    /// </summary>
    internal TvdbXmlReader()
    {

    }

    /// <summary>
    /// Extract a list of languages when the data has the format:
    /// <![CDATA[
    ///
    /// <?xml version="1.0" encoding="UTF-8" ?>
    /// <Languages>
    ///  <Language>
    ///    <name>Español</name>
    ///    <abbreviation>es</abbreviation>
    ///    <id>16</id>
    ///  </Language>
    /// </Languages>
    /// 
    /// ]]>
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    internal List<TvdbLanguage> ExtractLanguages(String _data)
    {
      XDocument xml = XDocument.Parse(_data);

      var allLanguages = from language in xml.Descendants("Language")
                         select new
                         {
                           name = language.Element("name").Value,
                           abbreviation = language.Element("abbreviation").Value,
                           id = language.Element("id").Value
                         };

      List<TvdbLanguage> retList = new List<TvdbLanguage>();
      foreach (var l in allLanguages)
      {
        TvdbLanguage lang = new TvdbLanguage();
        lang.Name = l.name;
        lang.Abbriviation = l.abbreviation;
        lang.Id = Util.Int32Parse(l.id);

        if (lang.Id != -99) retList.Add(lang);
      }
      return retList;
    }

    /// <summary>
    /// Extract a list of mirrors if the data has the format:
    /// <![CDATA[
    /// <?xml version="1.0" encoding="UTF-8" ?>
    /// <Mirrors>
    ///  <Mirror>
    ///    <id>1</id>
    ///    <mirrorpath>http://thetvdb.com</mirrorpath>
    ///    <typemask>7</typemask>
    ///  </Mirror>
    /// </Mirrors>
    /// ]]>
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    [Obsolete("Not used any more, however if won't delete the class since it could be useful at some point")]
    internal List<TvdbMirror> ExtractMirrors(String _data)
    {
      XDocument xml = XDocument.Parse(_data);

      var allLanguages = from language in xml.Descendants("Mirror")
                         select new
                         {
                           typemask = language.Element("typemask").Value,
                           mirrorpath = language.Element("mirrorpath").Value,
                           id = language.Element("id").Value
                         };

      List<TvdbMirror> retList = new List<TvdbMirror>();
      foreach (var l in allLanguages)
      {
        TvdbMirror lang = new TvdbMirror();
        lang.MirrorPath = new Uri(l.mirrorpath);
        lang.TypeMask = Util.Int32Parse(l.typemask);
        lang.Id = Util.Int32Parse(l.id);

        if (lang.Id != -99) retList.Add(lang);
      }
      return retList;
    }

    /// <summary>
    /// Extract a list of series in the format:
    /// <![CDATA[
    /// <?xml version="1.0" encoding="UTF-8" ?>
    /// <Data>
    ///    <Series>
    ///       <id>73739</id>
    ///       <Actors>|Malcolm David Kelley|Jorge Garcia|Maggie Grace|...|</Actors>
    ///       <Airs_DayOfWeek>Thursday</Airs_DayOfWeek>
    ///       <Airs_Time>9:00 PM</Airs_Time>
    ///       <ContentRating>TV-14</ContentRating>
    ///       <FirstAired>2004-09-22</FirstAired>
    ///       <Genre>|Action and Adventure|Drama|Science-Fiction|</Genre>
    ///       <IMDB_ID>tt0411008</IMDB_ID>
    ///       <Language>en</Language>
    ///       <Network>ABC</Network>
    ///       <Overview>After Oceanic Air flight 815...</Overview>
    ///       <Rating>8.9</Rating>
    ///       <Runtime>60</Runtime>
    ///       <SeriesID>24313</SeriesID>
    ///       <SeriesName>Lost</SeriesName>
    ///       <Status>Continuing</Status>
    ///       <banner>graphical/24313-g2.jpg</banner>
    ///       <fanart>fanart/original/73739-1.jpg</fanart>
    ///       <lastupdated>1205694666</lastupdated>
    ///       <zap2it_id>SH672362</zap2it_id>
    ///    </Series>
    /// </Data>
    /// ]]>
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    internal List<TvdbSeries> ExtractSeries(String _data)
    {

      List<TvdbSeriesFields> tvdbInfo = ExtractSeriesFields(_data);
      List<TvdbSeries> retList = new List<TvdbSeries>();
      foreach (TvdbSeriesFields s in tvdbInfo)
      {
        TvdbSeries series = new TvdbSeries(s);

        if (!series.BannerPath.Equals(""))
        {
          series.Banners.Add(new TvdbSeriesBanner(series.Id, series.BannerPath, series.Language, TvdbSeriesBanner.Type.graphical));
        }

        if (!series.FanartPath.Equals(""))
        {
          series.Banners.Add(new TvdbFanartBanner(series.Id, series.FanartPath, series.Language));
        }

        if (!series.PosterPath.Equals(""))
        {
          series.Banners.Add(new TvdbPosterBanner(series.Id, series.PosterPath, series.Language));
        }
        retList.Add(series);
      }
      return retList;
    }

    /// <summary>
    /// Extract all the series fields that are available on thetvdb
    /// <![CDATA[
    /// <?xml version="1.0" encoding="UTF-8" ?>
    /// <Data>
    ///    <Series>
    ///       <id>73739</id>
    ///       <Actors>|Malcolm David Kelley|Jorge Garcia|Maggie Grace|...|</Actors>
    ///       <Airs_DayOfWeek>Thursday</Airs_DayOfWeek>
    ///       <Airs_Time>9:00 PM</Airs_Time>
    ///       <ContentRating>TV-14</ContentRating>
    ///       <FirstAired>2004-09-22</FirstAired>
    ///       <Genre>|Action and Adventure|Drama|Science-Fiction|</Genre>
    ///       <IMDB_ID>tt0411008</IMDB_ID>
    ///       <Language>en</Language>
    ///       <Network>ABC</Network>
    ///       <Overview>After Oceanic Air flight 815...</Overview>
    ///       <Rating>8.9</Rating>
    ///       <Runtime>60</Runtime>
    ///       <SeriesID>24313</SeriesID>
    ///       <SeriesName>Lost</SeriesName>
    ///       <Status>Continuing</Status>
    ///       <banner>graphical/24313-g2.jpg</banner>
    ///       <fanart>fanart/original/73739-1.jpg</fanart>
    ///       <lastupdated>1205694666</lastupdated>
    ///       <zap2it_id>SH672362</zap2it_id>
    ///    </Series>
    ///  
    /// </Data>
    /// ]]>
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    internal List<TvdbSeriesFields> ExtractSeriesFields(String _data)
    {
      //Stopwatch watch = new Stopwatch();
      //watch.Start();
      XDocument xml = XDocument.Parse(_data);

      var allSeries = from series in xml.Descendants("Series")
                      select new
                      {
                        Id = series.Element("id").Value,
                        Actors = series.Element("Actors").Value,
                        Airs_DayOfWeek = series.Element("Airs_DayOfWeek").Value,
                        Airs_Time = series.Element("Airs_Time").Value,
                        ContentRating = series.Element("ContentRating").Value,
                        FirstAired = series.Element("FirstAired").Value,
                        Genre = series.Element("Genre").Value,
                        IMDB_ID = series.Element("IMDB_ID").Value,
                        Language = series.Element("Language").Value,
                        Network = series.Element("Network").Value,
                        Overview = series.Element("Overview").Value,
                        Rating = series.Element("Rating").Value,
                        Runtime = series.Element("Runtime").Value,
                        SeriesID = series.Element("SeriesID").Value,
                        SeriesName = series.Element("SeriesName").Value,
                        Status = series.Element("Status").Value,
                        banner = series.Element("banner").Value,
                        fanart = series.Element("fanart").Value,
                        poster = series.Element("poster").Value,
                        lastupdated = series.Element("lastupdated").Value,
                        zap2it_id = series.Element("zap2it_id").Value
                      };

      List<TvdbSeriesFields> retList = new List<TvdbSeriesFields>();
      foreach (var s in allSeries)
      {
        TvdbSeriesFields series = new TvdbSeriesFields();
        series.Id = Util.Int32Parse(s.Id);
        series.Actors = Util.SplitTvdbString(s.Actors);
        series.AirsDayOfWeek = Util.GetDayOfWeek(s.Airs_DayOfWeek);
        series.AirsTime = s.Airs_Time.Equals("") ? new DateTime(1, 1, 1) : DateTime.Parse(s.Airs_Time.Replace(".", ":"));
        series.ContentRating = s.ContentRating;
        series.FirstAired = s.FirstAired.Equals("") ? new DateTime() : DateTime.Parse(s.FirstAired);
        series.Genre = Util.SplitTvdbString(s.Genre);
        series.ImdbId = s.IMDB_ID;
        series.Language = Util.ParseLanguage(s.Language);
        series.Network = s.Network;
        series.Overview = s.Overview;
        series.Rating = Util.DoubleParse(s.Rating);
        series.Runtime = Util.DoubleParse(s.Runtime);
        series.TvDotComId = Util.Int32Parse(s.SeriesID);
        series.SeriesName = s.SeriesName;
        series.Status = s.Status;
        series.BannerPath = s.banner;
        series.FanartPath = s.fanart;
        series.PosterPath = s.poster;
        series.LastUpdated = Util.UnixToDotNet(s.lastupdated);
        series.Zap2itId = s.zap2it_id;
        if (series.Id != -99) retList.Add(series);
      }

      //watch.Stop();
      //Log.Debug("Extracted " + retList.Count + " series in " + watch.ElapsedMilliseconds + " milliseconds");
      return retList;
    }

    /// <summary>
    /// Extract a list of episodes from the given data when the data has the following format:
    /// <![CDATA[
    ///  <?xml version="1.0" encoding="UTF-8" ?>
    ///  <Episode>
    ///      <id>332179</id>
    ///      <DVD_chapter></DVD_chapter>
    ///      <DVD_discid></DVD_discid>
    ///      <DVD_episodenumber></DVD_episodenumber>
    ///      <DVD_season></DVD_season>
    ///      <Director>|Joseph McGinty Nichol|</Director>
    ///      <EpisodeName>Chuck Versus the World</EpisodeName>
    ///      <EpisodeNumber>1</EpisodeNumber>
    ///      <FirstAired>2007-09-24</FirstAired>
    ///      <GuestStars>|Julia Ling|Vik Sahay|Mieko Hillman|</GuestStars>
    ///      <IMDB_ID></IMDB_ID>
    ///      <Language>English</Language>
    ///      <Overview>Chuck Bartowski is an average computer geek...</Overview>
    ///      <ProductionCode></ProductionCode>
    ///      <Rating>9.0</Rating>
    ///      <SeasonNumber>1</SeasonNumber>
    ///      <Writer>|Josh Schwartz|Chris Fedak|</Writer>
    ///      <absolute_number></absolute_number>
    ///      <airsafter_season></airsafter_season>
    ///      <airsbefore_episode></airsbefore_episode>
    ///      <airsbefore_season></airsbefore_season>
    ///      <filename>episodes/80348-332179.jpg</filename>
    ///      <lastupdated>1201292806</lastupdated>
    ///      <seasonid>27985</seasonid>
    ///      <seriesid>80348</seriesid>
    ///  </Episode>
    ///  ]]>
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    internal List<TvdbEpisode> ExtractEpisodes(String _data)
    {
      //Stopwatch watch = new Stopwatch();
      //watch.Start();
      XDocument xml = XDocument.Parse(_data);
      var allEpisodes = from episode in xml.Descendants("Episode")
                        select new
                        {
                          Id = episode.Element("id").Value,
                          Combined_episodenumber = episode.Elements("Combined_episodenumber").Count() == 1
                                                 ? episode.Element("Combined_episodenumber").Value : "0",
                          Combined_season = episode.Elements("Combined_season").Count() == 1
                                          ? episode.Element("Combined_season").Value : "0",
                          DVD_chapter = episode.Element("DVD_chapter").Value,
                          DVD_discid = episode.Element("DVD_discid").Value,
                          DVD_episodenumber = episode.Element("DVD_episodenumber").Value,
                          DVD_season = episode.Elements("DVD_season").Count() == 1
                                       ? episode.Element("DVD_season").Value : episode.Element("DVD_Season").Value,
                          Director = episode.Element("Director").Value,
                          EpisodeName = episode.Element("EpisodeName").Value,
                          EpisodeNumber = episode.Element("EpisodeNumber").Value,
                          FirstAired = episode.Element("FirstAired").Value,
                          GuestStars = episode.Element("GuestStars").Value,
                          IMDB_ID = episode.Element("IMDB_ID").Value,
                          Language = episode.Elements("Language").Count() == 1
                                     ? episode.Element("Language").Value : "en",
                          Overview = episode.Element("Overview").Value,
                          ProductionCode = episode.Element("ProductionCode").Value,
                          Rating = episode.Element("Rating").Value,
                          SeasonNumber = episode.Element("SeasonNumber").Value,
                          Writer = episode.Element("Writer").Value,
                          absolute_number = episode.Element("absolute_number").Value,
                          filename = episode.Element("filename").Value,
                          lastupdated = episode.Element("lastupdated").Value,
                          seasonid = episode.Element("seasonid").Value,
                          seriesid = episode.Element("seriesid").Value,
                          airsafter_season = episode.Elements("airsafter_season").Count() == 1
                                           ? episode.Element("airsafter_season").Value : "-99",
                          airsbefore_episode = episode.Elements("airsbefore_episode").Count() == 1
                                             ? episode.Element("airsbefore_episode").Value : "-99",
                          airsbefore_season = episode.Elements("airsbefore_season").Count() == 1
                                            ? episode.Element("airsbefore_season").Value : "-99"
                        };
      //Log.Debug("Parsed xml file in  " + watch.ElapsedMilliseconds + " milliseconds");
      List<TvdbEpisode> retList = new List<TvdbEpisode>();
      foreach (var e in allEpisodes)
      {
        TvdbEpisode ep = new TvdbEpisode();
        ep.Id = Util.Int32Parse(e.Id);
        ep.CombinedEpisodeNumber = Util.DoubleParse(e.Combined_episodenumber);
        ep.CombinedSeason = Util.DoubleParse(e.Combined_season);
        ep.DvdChapter = Util.Int32Parse(e.DVD_chapter);
        ep.DvdDiscId = Util.Int32Parse(e.DVD_discid);
        ep.DvdEpisodeNumber = Util.DoubleParse(e.DVD_episodenumber);
        ep.DvdSeason = Util.Int32Parse(e.DVD_season);
        ep.Directors = Util.SplitTvdbString(e.Director);
        ep.EpisodeName = e.EpisodeName;
        ep.EpisodeNumber = Util.Int32Parse(e.EpisodeNumber);
        ep.AirsAfterSeason = Util.Int32Parse(e.airsafter_season);
        ep.AirsBeforeEpisode = Util.Int32Parse(e.airsbefore_episode);
        ep.AirsBeforeSeason = Util.Int32Parse(e.airsbefore_season);
        try
        {
          ep.FirstAired = e.FirstAired.Equals("") ? new DateTime(1, 1, 1) : DateTime.Parse(e.FirstAired);
        }
        catch (Exception)
        {
          ep.FirstAired = new DateTime();
        }
        ep.GuestStars = Util.SplitTvdbString(e.GuestStars);
        ep.ImdbId = e.IMDB_ID;
        ep.Language = Util.ParseLanguage(e.Language);
        ep.Overview = e.Overview;
        ep.ProductionCode = e.ProductionCode;
        ep.Rating = Util.DoubleParse(e.Rating);
        ep.SeasonNumber = Util.Int32Parse(e.SeasonNumber);
        ep.Writer = Util.SplitTvdbString(e.Writer);
        ep.AbsoluteNumber = Util.Int32Parse(e.absolute_number);
        ep.BannerPath = e.filename;
        ep.Banner = new TvdbEpisodeBanner(ep.Id, ep.BannerPath);
        ep.LastUpdated = Util.UnixToDotNet(e.lastupdated);
        ep.SeasonId = Util.Int32Parse(e.seasonid);
        ep.SeriesId = Util.Int32Parse(e.seriesid);

        if (ep.Id != -99) retList.Add(ep);
      }

      //watch.Stop();
      //Log.Debug("Extracted " + retList.Count + " Episodes in " + watch.ElapsedMilliseconds + " milliseconds");
      return retList;

    }

    /// <summary>
    /// Extract list of updated series
    /// <![CDATA[
    /// <?xml version="1.0" encoding="UTF-8" ?>
    /// <Data time="1203923101">
    ///    <Series>
    ///      <id>71969</id>
    ///      <time>1203848965</time>
    ///    </Series>
    ///  </Data>
    ///  ]]>
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    internal List<TvdbSeries> ExtractSeriesUpdates(String _data)
    {

      XDocument xml = XDocument.Parse(_data);

      var allSeries = from series in xml.Descendants("Series")
                      where series.HasElements == true
                      select new TvdbSeries
                      {
                        Id = Util.Int32Parse(series.Element("id").Value),
                        LastUpdated = Util.UnixToDotNet(series.Element("time").Value)
                      };

      List<TvdbSeries> retList = new List<TvdbSeries>();
      foreach (TvdbSeries s in allSeries)
      {
        if (s != null && s.Id != -99) retList.Add(s);
      }

      return retList;
    }

    /// <summary>
    /// Extract the results of a series search with format:
    /// <![CDATA[
    /// <?xml version="1.0" encoding="UTF-8" ?>
    /// <Data>
    ///   <Series>
    ///      <seriesid>73739</seriesid>
    ///      <language>en</language>
    ///      <SeriesName>Lost</SeriesName>
    ///      <banner>graphical/24313-g2.jpg</banner>
    ///      <Overview>After Oceanic Air flight 815...</Overview>
    ///      <FirstAired>2004-09-22</FirstAired>
    ///      <IMDB_ID>tt0411008</IMDB_ID>
    ///      <zap2it_id>SH672362</zap2it_id>
    ///      <id>73739</id>
    ///   </Series>
    /// </Data>
    /// ]]>
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    internal List<TvdbSearchResult> ExtractSeriesSearchResults(String _data)
    {

      XDocument xml = XDocument.Parse(_data);

      var allSeries = from series in xml.Descendants("Series")
                      where series.HasElements == true
                      select new
                      {
                        Id = Util.Int32Parse(series.Element("seriesid").Value),
                        FirstAired = series.Element("FirstAired") != null ? series.Element("FirstAired").Value : "",
                        Language = series.Element("language") != null ? series.Element("language").Value : "",
                        Overview = series.Element("Overview") != null ? series.Element("Overview").Value : "",
                        SeriesName = series.Element("SeriesName") != null ? series.Element("SeriesName").Value : "",
                        IMDB_ID = series.Element("IMDB_ID") != null ? series.Element("IMDB_ID").Value : "",
                        BannerPath = series.Element("banner") != null ? series.Element("banner").Value : ""
                      };

      List<TvdbSearchResult> retList = new List<TvdbSearchResult>();
      foreach (var s in allSeries)
      {
        TvdbSearchResult res = new TvdbSearchResult();
        res.Id = s.Id;
        res.ImdbId = s.IMDB_ID;
        if (!s.FirstAired.Equals("")) res.FirstAired = DateTime.Parse(s.FirstAired);
        if (!s.Language.Equals("")) res.Language = Util.ParseLanguage(s.Language);
        res.SeriesName = s.SeriesName;
        res.Overview = s.Overview;
        if (!s.BannerPath.Equals(""))
        {
          res.Banner = new TvdbSeriesBanner(0, s.BannerPath, null, TvdbSeriesBanner.Type.none);
        }
        else
        {
          res.Banner = new TvdbSeriesBanner(s.Id, null, null, TvdbSeriesBanner.Type.none);
        }
        retList.Add(res);
      }

      return retList;
    }

    /// <summary>
    /// Exctract the series favorites
    /// <![CDATA[
    /// <?xml version="1.0" encoding="UTF-8" ?>
    /// <Favorites>
    ///   <Series>73067</Series>
    ///   <Series>78957</Series>
    ///   <Series>75340</Series>
    ///   <Series>72218</Series>
    ///   <Series>73244</Series>
    ///   <Series>75397</Series>
    /// </Favorites>
    /// ]]>
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    internal List<int> ExtractSeriesFavorites(String _data)
    {

      XDocument xml = XDocument.Parse(_data);

      var allSeries = from series in xml.Descendants("Series")
                      select new
                      {
                        Id = Util.Int32Parse(series.Value),
                      };

      List<int> retList = new List<int>();
      foreach (var s in allSeries)
      {
        if (s.Id != -99) retList.Add(s.Id);
      }

      return retList;
    }

    /// <summary>
    /// Extract a rating from the data in the format
    /// <![CDATA[
    /// <?xml version="1.0" encoding="UTF-8" ?>
    /// <Data>
    ///  <Series>
    ///    <Rating>7.5</Rating>
    ///  </Series>
    /// </Data>
    /// ]]>
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    internal double ExtractRating(String _data)
    {
      XDocument xml = XDocument.Parse(_data);

      var ratings = from series in xml.Descendants("Rating")
                    select new
                    {
                      rating = series.Value
                    };
      if (ratings.Count() == 1 && ratings.ElementAt(0).rating != null)
      {
        return Util.DoubleParse(ratings.ElementAt(0).rating);
      }
      else
      {
        return -99;
      }
    }

    /// <summary>
    /// Extract the updated episodes from the data in the format:
    /// 
    /// <![CDATA[
    /// <?xml version="1.0" encoding="UTF-8" ?>
    /// <Data time="1203923101">
    ///    <Episode>
    ///      <id>326268</id>
    ///      <time>1203848662</time>
    ///    </Episode>
    ///  </Data>
    ///  ]]>
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    internal List<TvdbEpisode> ExtractEpisodeUpdates(String _data)
    {
      XDocument xml = XDocument.Parse(_data);
      var allEpisodes = from episode in xml.Descendants("Episode")
                        select new TvdbEpisode
                        {
                          Id = Util.Int32Parse(episode.Element("id").Value),
                          LastUpdated = Util.UnixToDotNet(episode.Element("time").Value),
                          SeriesId = Util.Int32Parse(episode.Element("Series").Value)
                        };

      List<TvdbEpisode> retList = new List<TvdbEpisode>();
      foreach (TvdbEpisode e in allEpisodes)
      {
        if (e.Id != -99) retList.Add(e);
      }

      return retList;

    }

    /// <summary>
    /// Extract the data of updated banners
    /// 
    /// <![CDATA[
    /// <?xml version="1.0" encoding="UTF-8" ?>
    /// <Data time="1203923101">
    ///    <Banner>
    ///      <SeasonNum>1</SeasonNum>
    ///      <Series>79302</Series>
    ///      <format>standard</format>
    ///      <language>en</language>
    ///      <path>seasons/79302-1.jpg</path>
    ///      <type>season</type>
    ///    </Banner>
    ///  </Data>
    /// ]]>
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    internal List<TvdbBanner> ExtractBannerUpdates(String _data)
    {
      //todo: banner update -> problem is update.xml doesn't contain all information for fanart
      //Stopwatch watch = new Stopwatch();
      //watch.Start();

      XDocument xml = XDocument.Parse(_data);
      List<TvdbBanner> retList = new List<TvdbBanner>();

      //Extract the fanart banners
      var allEpisodes = from episode in xml.Descendants("Banner")
                        where episode.Element("type").Value.Equals("fanart")
                        select new TvdbFanartBanner
                        {
                          Id = -99,//Util.Int32Parse(episode.Element("Series").Value),
                          BannerPath = episode.Element("path").Value,
                          VignettePath = episode.Element("path").Value.Replace("/original/", "/vignette/"),
                          ThumbPath = "_cache/" + episode.Element("path").Value,
                          Resolution = Util.ParseResolution(episode.Element("format").Value),
                          //Colors = Util.ParseColors(episode.Element("Colors").Value),
                          //Language = Util.ParseLanguage(episode.Element("Language").Value)
                          SeriesId = Util.Int32Parse(episode.Element("Series").Value),
                          LastUpdated = Util.UnixToDotNet(episode.Element("time").Value)
                        };

      foreach (TvdbBanner e in allEpisodes)
      {
        retList.Add(e);
      }

      //Extract the season banners
      var allBanners = from banner in xml.Descendants("Banner")
                       where banner.Element("type").Value.Equals("season")
                       select new TvdbSeasonBanner
                       {
                         Id = -99,//Util.Int32Parse(banner.Element("Series").Value),
                         BannerPath = banner.Element("path").Value,
                         Season = Util.Int32Parse(banner.Element("SeasonNum").Value),
                         BannerType = Util.ParseSeasonBannerType(banner.Element("format").Value),
                         Language = Util.ParseLanguage(banner.Element("language").Value),
                         SeriesId = Util.Int32Parse(banner.Element("Series").Value),
                         LastUpdated = Util.UnixToDotNet(banner.Element("time").Value)
                       };

      foreach (TvdbBanner e in allBanners)
      {
        retList.Add(e);
      }

      //Extract the series banners
      var allBanners2 = from banner in xml.Descendants("Banner")
                        where banner.Element("type").Value.Equals("series")
                        select new TvdbSeriesBanner
                        {
                          Id = -99,//Util.Int32Parse(banner.Element("Series").Value),
                          BannerPath = banner.Element("path").Value,
                          BannerType = Util.ParseSeriesBannerType(banner.Element("format").Value),
                          Language = Util.ParseLanguage(banner.Element("language").Value),
                          SeriesId = Util.Int32Parse(banner.Element("Series").Value),
                          LastUpdated = Util.UnixToDotNet(banner.Element("time").Value)
                        };

      foreach (TvdbBanner e in allBanners2)
      {
        retList.Add(e);
      }

      //Extract the poster banners
      var allPosters = from banner in xml.Descendants("Banner")
                       where banner.Element("type").Value.Equals("poster")
                       select new TvdbPosterBanner
                       {
                         Id = -99,//Util.Int32Parse(banner.Element("Series").Value),
                         BannerPath = banner.Element("path").Value,
                         Resolution = Util.ParseResolution(banner.Element("format").Value),
                         Language = TvdbLanguage.UniversalLanguage,
                         SeriesId = Util.Int32Parse(banner.Element("Series").Value),
                         LastUpdated = Util.UnixToDotNet(banner.Element("time").Value)
                       };

      foreach (TvdbPosterBanner e in allPosters)
      {
        retList.Add(e);
      }
      //watch.Stop();
      //Log.Debug("Extracted " + retList.Count + " bannerupdates in " + watch.ElapsedMilliseconds + " milliseconds");
      return retList;
    }

    /// <summary>
    /// Extract the update time from data
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    internal DateTime ExtractUpdateTime(string _data)
    {
      XDocument xml = XDocument.Parse(_data);
      var updateTime = from episode in xml.Descendants("Data")
                       select new
                       {
                         time = episode.Attribute("time").Value
                       };
      foreach (var d in updateTime)
      {
        if (d.time != "")
        {
          return Util.UnixToDotNet(d.time);
        }
      }
      return new DateTime(1, 1, 1);
    }


    /// <summary>
    /// Extract a list of banners from the data when the data has the format:
    /// <![CDATA[
    /// <?xml version="1.0" encoding="UTF-8" ?>
    /// <Banners>
    ///    <Banner>
    ///       <id>20106</id>
    ///       <BannerPath>fanart/original/73739-1.jpg</BannerPath>
    ///       <VignettePath>fanart/vignette/73739-1.jpg</VignettePath>
    ///       <ThumbnailPath>_cache/fanart/original/73739-1.jpg</ThumbnailPath>
    ///       <BannerType>fanart</BannerType>
    ///       <BannerType2>1920x1080</BannerType2>
    ///       <Colors>|68,69,59|69,70,58|78,78,68|</Colors>
    ///       <Language>en</Language>
    ///    </Banner>
    ///    <Banner>
    ///       <id>18953</id>
    ///       <BannerPath>seasons/73739-2-2.jpg</BannerPath>
    ///       <BannerType>season</BannerType>
    ///       <BannerType2>season</BannerType2>
    ///       <Language>es</Language>
    ///       <Season>2</Season>
    ///    </Banner>
    ///    <Banner>
    ///       <id>9529</id>
    ///       <BannerPath>graphical/73739-g.jpg</BannerPath>
    ///       <BannerType>series</BannerType>
    ///       <BannerType2>graphical</BannerType2>
    ///       <Language>en</Language>
    ///    </Banner>
    /// </Banners>
    /// ]]>
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    internal List<TvdbBanner> ExtractBanners(String _data)
    {
      //Stopwatch watch = new Stopwatch();
      //watch.Start();

      XDocument xml = XDocument.Parse(_data);
      List<TvdbBanner> retList = new List<TvdbBanner>();

      //Extract the fanart banners
      var allFanartBanners =  from banner in xml.Descendants("Banner")
                              where banner.Element("BannerType").Value.Equals("fanart")
                              select new TvdbFanartBanner
                              {
                                Id = banner.Element("id") != null ? Util.Int32Parse(banner.Element("id").Value) : -99,
                                BannerPath = banner.Element("BannerPath") != null ? banner.Element("BannerPath").Value : "",
                                VignettePath = banner.Element("id") != null ? banner.Element("VignettePath").Value : "",
                                ThumbPath = banner.Element("ThumbnailPath") != null ? banner.Element("ThumbnailPath").Value : "",
                                Resolution = banner.Element("BannerType2") != null ? 
                                             Util.ParseResolution(banner.Element("BannerType2").Value) : new Point(),
                                Colors = banner.Element("Colors") != null ? Util.ParseColors(banner.Element("Colors").Value) : null,
                                Language = banner.Element("Language") != null ? 
                                           Util.ParseLanguage(banner.Element("Language").Value) : TvdbLanguage.DefaultLanguage,
                                ContainsSeriesName = banner.Element("SeriesName") != null ?
                                                     Util.ParseBoolean(banner.Element("SeriesName").Value) : false,
                                LastUpdated = banner.Element("LastUpdated") != null ? 
                                              Util.UnixToDotNet(banner.Element("LastUpdated").Value) : DateTime.Now
                              };

      foreach (TvdbBanner e in allFanartBanners)
      {
        if (e.Id != -99) retList.Add(e);
      }

      //Extract the season banners
      var allSeasonBanners = from banner in xml.Descendants("Banner")
                             where banner.Element("BannerType").Value.Equals("season")
                             select new TvdbSeasonBanner
                             {
                               Id = Util.Int32Parse(banner.Element("id").Value),
                               BannerPath = banner.Element("BannerPath").Value,
                               Season = Util.Int32Parse(banner.Element("Season").Value),
                               BannerType = Util.ParseSeasonBannerType(banner.Element("BannerType2").Value),
                               Language = Util.ParseLanguage(banner.Element("Language").Value),
                               LastUpdated = banner.Element("LastUpdated") != null ?
                                             Util.UnixToDotNet(banner.Element("LastUpdated").Value) : DateTime.Now
                             };

      foreach (TvdbBanner e in allSeasonBanners)
      {
        if (e.Id != -99) retList.Add(e);
      }

      //Extract the series banners
      var allSeriesBanners =  from banner in xml.Descendants("Banner")
                              where banner.Element("BannerType").Value.Equals("series")
                              select new TvdbSeriesBanner
                              {
                                Id = Util.Int32Parse(banner.Element("id").Value),
                                BannerPath = banner.Element("BannerPath").Value,
                                BannerType = Util.ParseSeriesBannerType(banner.Element("BannerType2").Value),
                                Language = Util.ParseLanguage(banner.Element("Language").Value),
                                LastUpdated = banner.Element("LastUpdated") != null ?
                                              Util.UnixToDotNet(banner.Element("LastUpdated").Value) : DateTime.Now
                              };

      foreach (TvdbBanner e in allSeriesBanners)
      {
        if (e.Id != -99) retList.Add(e);
      }

      //Extract the poster banners
      var allPosterBanners = from banner in xml.Descendants("Banner")
                             where banner.Element("BannerType").Value.Equals("poster")
                             select new TvdbPosterBanner
                             {

                               Id = Util.Int32Parse(banner.Element("id").Value),
                               BannerPath = banner.Element("BannerPath").Value,
                               Resolution = Util.ParseResolution(banner.Element("BannerType2").Value),
                               Language = Util.ParseLanguage(banner.Element("Language").Value),
                               LastUpdated = banner.Element("LastUpdated") != null ?
                                             Util.UnixToDotNet(banner.Element("LastUpdated").Value) : DateTime.Now
                             };

      foreach (TvdbPosterBanner e in allPosterBanners)
      {
        if (e.Id != -99) retList.Add(e);
      }
      //watch.Stop();
      //Log.Debug("Extracted " + retList.Count + " banners in " + watch.ElapsedMilliseconds + " milliseconds");
      return retList;
    }

    /// <summary>
    /// Extract a list of actors when the data has the format:
    /// <![CDATA[
    /// <?xml version="1.0" encoding="UTF-8" ?>
    /// <Actors>
    ///   <Actor>
    ///     <id>22017</id>
    ///     <Image>actors/22017.jpg</Image>
    ///     <Name>Zachary Levi</Name>
    ///     <Role>Chuck Bartowski</Role>
    ///     <SortOrder>0</SortOrder>
    ///   </Actor>
    /// </Actors>
    /// ]]>
    /// </summary>
    /// <param name="_data">data</param>
    /// <returns>List of actors</returns>
    internal List<TvdbActor> ExtractActors(String _data)
    {
      //Stopwatch watch = new Stopwatch();
      //watch.Start();

      XDocument xml = XDocument.Parse(_data);
      List<TvdbBanner> retList = new List<TvdbBanner>();
      var allActors = from episode in xml.Descendants("Actor")
                      select new
                      {

                        Id = episode.Element("id").Value,
                        Image = episode.Element("Image").Value,
                        Name = episode.Element("Name").Value,
                        Role = episode.Element("Role").Value,
                        SortOrder = episode.Element("SortOrder").Value
                      };
      List<TvdbActor> actorList = new List<TvdbActor>();
      foreach (var a in allActors)
      {
        TvdbActor actor = new TvdbActor();
        actor.Id = Util.Int32Parse(a.Id);
        actor.Name = a.Name;
        actor.Role = a.Role;
        actor.SortOrder = Util.Int32Parse(a.SortOrder);

        TvdbActorBanner banner = new TvdbActorBanner();
        banner.Id = actor.Id;
        banner.BannerPath = a.Image;
        actor.ActorImage = banner;
        if (actor.Id != -99)
        {
          actorList.Add(actor);
        }
      }
      //watch.Stop();
      //Log.Debug("Extracted " + actorList.Count + " actors in " + watch.ElapsedMilliseconds + " milliseconds");
      return actorList;
    }

    /// <summary>
    /// Extract user data from
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    internal List<TvdbUser> ExtractUser(String _data)
    {
      //Stopwatch watch = new Stopwatch();
      //watch.Start();
      XDocument xml = XDocument.Parse(_data);
      List<TvdbBanner> retList = new List<TvdbBanner>();
      var allUsers = from episode in xml.Descendants("User")
                     select new
                     {

                       Identifier = episode.Element("Identifier").Value,
                       Name = episode.Element("Name").Value,
                       Favorites = episode.Element("Favorites"),
                       Preferred = episode.Element("PreferredLanguage")
                     };

      List<TvdbUser> userList = new List<TvdbUser>();
      foreach (var a in allUsers)
      {
        TvdbUser user = new TvdbUser();
        user.UserIdentifier = a.Identifier;
        user.UserName = a.Name;
        user.UserPreferredLanguage = a.Preferred.HasAttributes ?
                                     Util.ParseLanguage(a.Preferred.FirstAttribute.NextAttribute.Value) :
                                     TvdbLanguage.DefaultLanguage;
        List<int> favList = new List<int>();
        foreach (String f in a.Favorites.Value.Split(','))
        {
          int val;
          if (Int32.TryParse(f, out val))
          {
            favList.Add(val);
          }
        }
        user.UserFavorites = favList;
        userList.Add(user);
      }
      //watch.Stop();
      //Log.Debug("Extracted " + userList.Count + " actors in " + watch.ElapsedMilliseconds + " milliseconds");
      return userList;
    }

    /// <summary>
    /// Extract a list of series ratings
    /// 
    /// The xml file is in the following format:
    /// <![CDATA[
    /// <?xml version="1.0" encoding="UTF-8" ?> 
    /// <Data>
    ///   <Series>
    ///     <seriesid>80344</seriesid> 
    ///     <UserRating>7</UserRating> 
    ///     <CommunityRating>8.3224</CommunityRating> 
    ///   </Series>
    ///   <Series>
    ///     <seriesid>72227</seriesid> 
    ///     <UserRating>8</UserRating> 
    ///     <CommunityRating>8.3224</CommunityRating> 
    ///   </Series>
    /// </Data>
    /// ]]>
    /// </summary>
    /// <param name="_data">The xml content</param>
    /// <param name="_type">The item type for the ratings</param>
    /// <returns></returns>
    internal Dictionary<int, TvdbRating> ExtractRatings(string _data, TvdbRating.ItemType _type)
    {
      XDocument xml = XDocument.Parse(_data);
      String itemType = null;
      String idDefinition = null;
      switch (_type)
      {
        case TvdbRating.ItemType.Episode:
          itemType = "Episode";
          idDefinition = "id";
          break;
        case TvdbRating.ItemType.Series:
          itemType = "Series";
          idDefinition = "seriesid";
          break;
        default:
          return null;
      }

      var allRatings = from episode in xml.Descendants(itemType)
                       select new
                       {
                         SeriesId = Util.Int32Parse(episode.Element(idDefinition).Value),
                         UserRating = Util.Int32Parse(episode.Element("UserRating").Value),
                         CommunityRating = Util.DoubleParse(episode.Element("CommunityRating").Value)
                       };

      Dictionary<int, TvdbRating> retList = new Dictionary<int, TvdbRating>();
      foreach (var r in allRatings)
      {
        TvdbRating rating = new TvdbRating();
        rating.UserRating = r.UserRating;
        rating.CommunityRating = r.CommunityRating;
        rating.RatingItemType = _type;
        if (r.SeriesId != -99 && !retList.ContainsKey(r.SeriesId)) retList.Add(r.SeriesId, rating);
      }
      return retList;
    }
  }
}
