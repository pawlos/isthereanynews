namespace IsThereAnyNews.DataAccess.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Dtos;

    using EntityFramework;
    using EntityFramework.Models.Entities;

    using IsThereAnyNews.EntityFramework.Models.Events;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.ProjectionModels.Mess;
    using IsThereAnyNews.SharedData;

    public class EntityRepository: IEntityRepository
    {
        private readonly ItanDatabaseContext database;

        private readonly IMapper mapper;

        public EntityRepository(ItanDatabaseContext database, IMapper mapper)
        {
            this.database = database;
            this.mapper = mapper;
        }

        public void AddCommentRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest(userId, id, modelStreamType, FeatureRequestType.AddComment);
            this.SaveFeatureRequestToDatabase(featureRequest);
        }

        public void AddEventRssSkipped(long cui, List<long> ids)
        {
            var eventsToSave =
                ids.Select(
                    id =>
                        new EventRssUserInteraction
                        {
                            UserId = cui,
                            RssEntryId = id,
                            InteractionType = InteractionType.Skipped
                        });

            this.database.EventsRssUserInteraction.AddRange(eventsToSave);
            this.database.SaveChanges();
        }

        public void AddEventRssViewed(long currentUserId, long rssToReadId)
        {
            var eventRssViewed = new EventRssUserInteraction
            {
                RssEntryId = rssToReadId,
                UserId = currentUserId,
                InteractionType = InteractionType.Clicked
            };

            this.database.EventsRssUserInteraction.Add(eventRssViewed);
            this.database.SaveChanges();
        }

        public void AddFullArticleRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest(userId, id, modelStreamType, FeatureRequestType.FullArticle);
            this.SaveFeatureRequestToDatabase(featureRequest);
        }

        public void AddNotReadRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest(userId, id, modelStreamType, FeatureRequestType.NotRead);
            this.SaveFeatureRequestToDatabase(featureRequest);
        }

        public void AddReadLaterRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest(userId, id, modelStreamType, FeatureRequestType.ReadLater);
            this.SaveFeatureRequestToDatabase(featureRequest);
        }

        public void AddShareRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest(userId, id, modelStreamType, FeatureRequestType.Share);
            this.SaveFeatureRequestToDatabase(featureRequest);
        }

        public List<RssChannel> AddToGlobalSpace(long submitterId, List<RssChannel> importFromUpload)
        {
            var rssChannels = importFromUpload.Select(channel => new RssChannel(channel.Url, channel.Title, submitterId)).ToList();
            this.database.RssChannels.AddRange(rssChannels);
            this.database.SaveChanges();
            return rssChannels;
        }

        public void AddVoteDownRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest(userId, id, modelStreamType, FeatureRequestType.VoteDown);
            this.SaveFeatureRequestToDatabase(featureRequest);
        }

        public void AddVoteUpRequestByUserForArticle(long userId, StreamType modelStreamType, long id)
        {
            var featureRequest = new FeatureRequest(userId, id, modelStreamType, FeatureRequestType.Voteup);
            this.SaveFeatureRequestToDatabase(featureRequest);
        }

        public void AssignUserRole(long currentUserId)
        {
            var userRole = new UserRole() { RoleType = ItanRole.User, UserId = currentUserId };
            this.database.UserRoles.Add(userRole);
            this.database.SaveChanges();
        }

        //public void Blah()
        //{
        //    var x = new[]
        //                {
        //                    "http://feeds.reuters.com/news/artsculture", "http://feeds.reuters.com/reuters/businessNews",
        //                    "http://feeds.reuters.com/reuters/companyNews",
        //                    "http://feeds.reuters.com/reuters/entertainment",
        //                    "http://feeds.reuters.com/reuters/environment",
        //                    "http://feeds.reuters.com/reuters/healthNews", "http://feeds.reuters.com/reuters/lifestyle",
        //                    "http://feeds.reuters.com/news/reutersmedia", "http://feeds.reuters.com/news/wealth",
        //                    "http://feeds.reuters.com/reuters/MostRead",
        //                    "http://feeds.reuters.com/reuters/oddlyEnoughNews",
        //                    "http://feeds.reuters.com/ReutersPictures", "http://feeds.reuters.com/reuters/peopleNews",
        //                    "http://feeds.reuters.com/Reuters/PoliticsNews",
        //                    "http://feeds.reuters.com/reuters/scienceNews",
        //                    "http://feeds.reuters.com/reuters/sportsNews",
        //                    "http://feeds.reuters.com/reuters/technologyNews",
        //                    "http://feeds.reuters.com/reuters/topNews", "http://feeds.reuters.com/Reuters/domesticNews",
        //                    "http://feeds.reuters.com/Reuters/worldNews",
        //                    "http://feeds.reuters.com/reuters/bankruptcyNews",
        //                    "http://feeds.reuters.com/reuters/bondsNews", "http://feeds.reuters.com/news/deals",
        //                    "http://feeds.reuters.com/news/economy",
        //                    "http://feeds.reuters.com/reuters/globalmarketsNews",
        //                    "http://feeds.reuters.com/news/hedgefunds", "http://feeds.reuters.com/reuters/hotStocksNews",
        //                    "http://feeds.reuters.com/reuters/mergersNews",
        //                    "http://feeds.reuters.com/reuters/governmentfilingsNews",
        //                    "http://feeds.reuters.com/reuters/summitNews",
        //                    "http://feeds.reuters.com/reuters/USdollarreportNews",
        //                    "http://feeds.reuters.com/news/usmarkets",
        //                    "http://feeds.reuters.com/reuters/basicmaterialsNews",
        //                    "http://feeds.reuters.com/reuters/cyclicalconsumergoodsNews",
        //                    "http://feeds.reuters.com/reuters/USenergyNews",
        //                    "http://feeds.reuters.com/reuters/environment",
        //                    "http://feeds.reuters.com/reuters/financialsNews",
        //                    "http://feeds.reuters.com/reuters/UShealthcareNews",
        //                    "http://feeds.reuters.com/reuters/industrialsNews",
        //                    "http://feeds.reuters.com/reuters/USmediaDiversifiedNews",
        //                    "http://feeds.reuters.com/reuters/noncyclicalconsumergoodsNews",
        //                    "http://feeds.reuters.com/reuters/technologysectorNews",
        //                    "http://feeds.reuters.com/reuters/utilitiesNews",
        //                    "http://feeds.reuters.com/reuters/blogs/FinancialRegulatoryForum",
        //                    "http://feeds.reuters.com/reuters/blogs/GlobalInvesting",
        //                    "http://feeds.reuters.com/reuters/blogs/HugoDixon",
        //                    "http://feeds.reuters.com/reuters/blogs/India",
        //                    "http://feeds.reuters.com/reuters/blogs/JamesSaft",
        //                    "http://feeds.reuters.com/reuters/blogs/macroscope",
        //                    "http://feeds.reuters.com/reuters/blogs/mediafile",
        //                    "http://feeds.reuters.com/reuters/blogs/newsmaker",
        //                    "http://feeds.reuters.com/reuters/blogs/photo",
        //                    "http://feeds.reuters.com/reuters/blogs/SummitNotebook",
        //                    "http://feeds.reuters.com/reuters/blogs/talesfromthetrail",
        //                    "http://feeds.reuters.com/reuters/blogs/the-great-debate",
        //                    "http://feeds.reuters.com/UnstructuredFinance",
        //                    "http://feeds.reuters.com/reuters/USVideoBreakingviews",
        //                    "http://feeds.reuters.com/reuters/USVideoBusiness",
        //                    "http://feeds.reuters.com/reuters/USVideoBusinessTravel",
        //                    "http://feeds.reuters.com/reuters/USVideoChrystiaFreeland",
        //                    "http://feeds.reuters.com/reuters/USVideoEntertainment",
        //                    "http://feeds.reuters.com/reuters/USVideoEnvironment",
        //                    "http://feeds.reuters.com/reuters/USVideoFelixSalmon",
        //                    "http://feeds.reuters.com/reuters/USVideoGigaom",
        //                    "http://feeds.reuters.com/reuters/USVideoLifestyle",
        //                    "http://feeds.reuters.com/reuters/USVideoMostWatched",
        //                    "http://feeds.reuters.com/reuters/USVideoLatest",
        //                    "http://feeds.reuters.com/reuters/USVideoNewsmakers",
        //                    "http://feeds.reuters.com/reuters/USVideoOddlyEnough",
        //                    "http://feeds.reuters.com/reuters/USVideoPersonalFinance",
        //                    "http://feeds.reuters.com/reuters/USVideoPolitics",
        //                    "http://feeds.reuters.com/reuters/USVideoRoughCuts",
        //                    "http://feeds.reuters.com/reuters/USVideoSmallBusiness",
        //                    "http://feeds.reuters.com/reuters/USVideoTechnology",
        //                    "http://feeds.reuters.com/reuters/USVideoTopNews",
        //                    "http://feeds.reuters.com/reuters/USVideoWorldNews", "http://rss.cnn.com/rss/edition.rss",
        //                    "http://rss.cnn.com/rss/edition_world.rss", "http://rss.cnn.com/rss/edition_africa.rss",
        //                    "http://rss.cnn.com/rss/edition_americas.rss", "http://rss.cnn.com/rss/edition_asia.rss",
        //                    "http://rss.cnn.com/rss/edition_europe.rss", "http://rss.cnn.com/rss/edition_meast.rss",
        //                    "http://rss.cnn.com/rss/edition_us.rss",
        //                    "http://rss.cnn.com/rss/money_news_international.rss",
        //                    "http://rss.cnn.com/rss/edition_technology.rss", "http://rss.cnn.com/rss/edition_space.rss",
        //                    "http://rss.cnn.com/rss/edition_entertainment.rss",
        //                    "http://rss.cnn.com/rss/edition_sport.rss", "http://rss.cnn.com/rss/edition_football.rss",
        //                    "http://rss.cnn.com/rss/edition_golf.rss", "http://rss.cnn.com/rss/edition_motorsport.rss",
        //                    "http://rss.cnn.com/rss/edition_tennis.rss", "http://rss.cnn.com/rss/edition_travel.rss",
        //                    "http://rss.cnn.com/rss/cnn_freevideo.rss", "http://rss.cnn.com/rss/cnn_latest.rss",
        //                    "https://michalzajac.me/atom.xml", "http://aneta-bielska.github.io/feed.xml",
        //                    "https://medium.com/feed/", "http://maque.github.io/feed.xml",
        //                    "http://dev.nitwoe.com/feed.xml", "http://lukasstankiewicz.github.io/feed.xml",
        //                    "http://dev.kielczykowski.pl/feed.xml", "http://klimek.link/blog/feed/",
        //                    "http://jakubdziworski.github.io/feed.xml", "http://mickl.net/feed/",
        //                    "https://jacek.migdal.pl/feed.xml", "http://blog.e-polecanie.pl/feed/",
        //                    "http://dodocs.azurewebsites.net/rss", "http://dev.mensfeld.pl/feed/",
        //                    "http://www.feliszewski.eu/feed/", "https://art-of-ai.com/feed/",
        //                    "http://www.dobreprogramy.pl/djfoxer", "http://czekanski.info/feed/",
        //                    "https://tomoitblog.wordpress.com/feed/", "http://codingpersona.com/feed/",
        //                    "http://devfirststeps.blog.pl/feed/", "http://doriansobacki.pl/feed/",
        //                    "http://namiekko.pl/feed/", "http://parkowanko.blogspot.com/feeds/posts/default",
        //                    "http://blog.gonek.net/feed/", "http://kotprogramistyczny.pl/feed/",
        //                    "https://beabest.wordpress.com/feed/", "http://blog.yellowmoleproductions.pl/feed/",
        //                    "http://gronek.gq/feed/", "http://piatkosia.k4be.pl/wordpress/?feed=rss2",
        //                    "http://negativeprogrammer.blogspot.com/feeds/posts/default", "http://devbochenek.pl/feed/",
        //                    "https://dziewczynazpytonem.wordpress.com/feed/", "http://blog.chyla.org/rss",
        //                    "http://szymekk.me/blog/feed/", "http://www.dedlajn.pl/feeds/posts/default",
        //                    "http://filipcinik.azurewebsites.net/index.php/feed/",
        //                    "http://emigd.azurewebsites.net/feed.xml", "http://www.devanarch.com/feed/",
        //                    "http://knowakowski.azurewebsites.net/feed/", "http://www.adambac.com/feed.xml",
        //                    "http://tomaszkorecki.com/feed/", "http://www.sebastiangruchacz.pl/feed/",
        //                    "http://paweltymura.pl/feed/", "http://www.pyrzyk.net/feed/",
        //                    "http://marcinszyszka.pl/feed/", "http://blog.kars7e.io/feed.xml",
        //                    "http://dyzur.blogspot.com/feeds/posts/default",
        //                    "http://mateorobiapke.blogspot.com/feeds/posts/default", "http://tomasz.dudziak.eu/feed/",
        //                    "http://perceptrons.prv.pl/feed/", "https://dotnetcoder.wordpress.com/feed/",
        //                    "http://jakubskoczen.pl/feed/", "http://krzysztofzawistowski.azurewebsites.net/?feed=rss2",
        //                    "https://brinf.wordpress.com/feed/", "https://devblog.dymel.pl/feed/",
        //                    "http://koscielniak.me/post/index.xml", "http://liveshare.azurewebsites.net/feed/",
        //                    "http://blog.lantkowiak.pl/index.php/feed/", "http://pumiko.pl/feed.xml",
        //                    "http://kodikable.pl/rss", "http://halibuti.blogspot.com/feeds/posts/default",
        //                    "http://jaroslawstadnicki.pl/feed/", "https://duszekmestre.wordpress.com/feed/",
        //                    "http://koscielski.ninja/feed/", "http://matma.github.io/feed.xml",
        //                    "http://localwire.pl/feed/", "http://oxbow.pl/feed/", "http://adam.skobo.pl/?feed=rss2",
        //                    "http://it-michal-sitko.blogspot.com/feeds/posts/default",
        //                    "http://blog.simpleshop.pl/?feed=rss2", "http://kduszynski.pl/feed/",
        //                    "https://blog.scooletz.com/feed/", "http://dsp.katafrakt.me/feed.xml",
        //                    "http://www.karolpysklo.pl/?feed=rss2", "http://student.agh.edu.pl/~kefas/?feed=rss2",
        //                    "http://blog.kokosa.net/syndication.axd", "https://slaviannblog.wordpress.com/feed/",
        //                    "http://www.bodolsog.pl/devblog/feed/",
        //                    "http://michalgellert.blogspot.com/feeds/posts/default",
        //                    "http://feeds.feedburner.com/PassionateProgram", "http://donpiekarz.pl/feed.xml",
        //                    "http://piotr-wandycz.pl/feed/", "http://adamszneider.azurewebsites.net/feed/",
        //                    "http://www.xaocml.me/feed.xml", "http://sebcza.pl/feed/", "http://www.wearesicc.com/feed/",
        //                    "http://tomaszjarzynski.pl/feed/", "http://hryniewski.net/syndication.axd",
        //                    "http://toomanyitprojects.azurewebsites.net/feed/", "http://krystianbrozek.pl/feed/",
        //                    "http://rzeczybezinternetu.blogspot.com/feeds/posts/default",
        //                    "http://www.blog.plotnicki.net/?feed=rss2", "http://lukasz-jankowski.pl/feed/",
        //                    "http://www.md-techblog.net.pl/feed/", "http://swistak35.com/feed.xml",
        //                    "http://www.mikolajdemkow.pl/feed", "http://kubasz.esy.es/feed/", "http://szumiato.pl/feed/",
        //                    "http://mateuszstanek.pl/feed/", "http://metodprojekt.blogspot.com/feeds/posts/default",
        //                    "https://devprzemm.wordpress.com/feed/", "http://incodable.blogspot.com/feeds/posts/default",
        //                    "http://masakradev.tk/?feed=rss2", "http://crynkowski.pl/feed/", "http://manisero.net/feed/",
        //                    "http://programistka.net/feed/", "http://mbork.pl/?action=rss",
        //                    "http://lion.net.pl/blog/feed.xml", "http://www.diwebsity.com/feed/",
        //                    "http://bartoszrowinski.pl/feed/", "http://michalogluszka.pl/feed/",
        //                    "http://sprobujzmiany.blogspot.com/feeds/posts/default",
        //                    "https://adrianancymon.wordpress.com/feed/", "https://stitzdev.wordpress.com/feed/",
        //                    "http://cezary.mcwronka.com.hostingasp.pl/feed/", "http://chmielowski.net/feed/",
        //                    "https://damianwojcikblog.wordpress.com/feed/", "http://maciejskuratowski.com/feed/",
        //                    "http://tokenbattle.blogspot.com/feeds/posts/default", "http://napierala.org.pl/blog/feed/",
        //                    "http://nicholaszyl.net/feed/", "http://milena.mcwronka.com.hostingasp.pl/feed/",
        //                    "http://immora.azurewebsites.net/feed/", "http://blog.leszczynski.it/feed/",
        //                    "http://addictedtocreating.pl/feed/", "http://www.namekdev.net/feed/",
        //                    "https://barloblog.wordpress.com/feed/", "http://itcraftsman.pl/feed/",
        //                    "http://www.przemyslawowsianik.net/feed/", "http://www.ibpabisiak.pl/?feed=rss2",
        //                    "http://macieklesiczka.github.io/rss", "http://0x00antdot.blogspot.com/feeds/posts/default",
        //                    "http://kkustra.blogspot.com/feeds/posts/default", "http://msnowak.pl/feed/",
        //                    "http://bga.pl/index.php/feed/", "http://blog.jhossa.net/feed/", "http://nowas.pl/feed/",
        //                    "http://blog.degustudios.com/index.php/feed/", "http://jakubfalenczyk.com/feed/",
        //                    "https://citygame2016.wordpress.com/feed/",
        //                    "http://fogielpiotr.blogspot.com/feeds/posts/default",
        //                    "http://www.straightouttacode.net/rss", "http://blog.creyn.pl/feed/",
        //                    "http://ppkozlowski.pl/blog/feed/", "https://pablitoblogblog.wordpress.com/feed/",
        //                    "http://csharks.blogspot.com/feeds/posts/default",
        //                    "https://onehundredoneblog.wordpress.com/feed/", "http://mcupial.pl/feed/",
        //                    "http://www.select-iot.pl/feed/", "http://twitop.azurewebsites.net/index.php/feed/",
        //                    "http://brozanski.net/index.php/feed/", "http://arekbal.blogspot.com/feeds/posts/default",
        //                    "http://wezewkodzie.blogspot.com/feeds/posts/default", "http://codinghabit.pl/feed/",
        //                    "http://martanocon.com/?feed=rss2", "http://pawelrzepinski.azurewebsites.net/feed/",
        //                    "http://paweljurczynski.pl/feed/", "http://pgrzesiek.pl/feed/",
        //                    "http://krzyskowk.postach.io/feed.xml", "http://marcindrobik.pl/Home/rss",
        //                    "http://michal.muskala.eu/feed.xml", "http://damiankedzior.com/feed/",
        //                    "https://jporwol.wordpress.com/feed/", "http://takiarek.com/feed/",
        //                    "https://ourtownapp.wordpress.com/feed/", "http://ggajos.com/rss",
        //                    "http://commitandrun.pl/feed.xml", "http://czesio-w-it.2ap.pl/feed/",
        //                    "http://kreskadev.azurewebsites.net/rss", "https://werpuc.wordpress.com/feed/",
        //                    "http://jsdn.pl/feed/", "http://www.marcinwojdak.pl/?feed=rss2",
        //                    "http://www.malachowicz.org/?feed=rss2", "http://slavgamebrew.com/feed/",
        //                    "http://blog.exmoor.pl/feed", "https://koniecznuda.wordpress.com/feed/",
        //                    "https://krzysztofmorcinek.wordpress.com/feed/",
        //                    "http://blog.roobina.pl/?rss=516047c1-683c-4521-8ffd-143a0a546c85",
        //                    "http://www.arturnet.pl/feed/", "http://blog.waldemarbira.pl/feed/",
        //                    "http://justmypassions.pl/?feed=rss2", "http://blog.kurpio.com/feed/",
        //                    "http://cad.bane.pl/feed/", "http://codestorm.pl/feed/",
        //                    "http://charyzmatyczny-programista.blogspot.com/feeds/posts/default",
        //                    "https://odzeradokoderablog.wordpress.com/feed/", "http://lukasz-zborek.pl/feed/",
        //                    "https://ismenax.wordpress.com/feed/", "http://epascales.blogspot.com/feeds/posts/default",
        //                    "http://mariuszbartosik.com/feed/", "http://dragonet-therrax.blog.pl/feed/",
        //                    "http://www.sgierlowski.pl/posts/rss",
        //                    "http://moje-zagwostki.blogspot.com/feeds/posts/default",
        //                    "http://aragornziel.blogspot.com/feeds/posts/default",
        //                    "http://bartmalanczuk.github.io/feed.xml", "http://www.winiar.pl/blog/feed/",
        //                    "http://msaldak.pl/feed/", "http://krzysztofabramowicz.com/feed/",
        //                    "https://zerojedynka.wordpress.com/feed/", "https://kamilhawdziejuk.wordpress.com/feed/",
        //                    "http://paweldobrzanski.pl/feed", "http://polak.azurewebsites.net/rss",
        //                    "https://bizon7nt.github.io/feed.xml", "http://foreverframe.pl/feed/",
        //                    "https://bzaremba.wordpress.com/feed/", "http://marcinkowalczyk.pl/blog/feed/",
        //                    "http://www.webatelier.io/blog.xml", "http://rutkowski.in/feed/",
        //                    "http://jagielski.net/feed/", "http://plotzwi.com/feed/",
        //                    "https://netgwg.wordpress.com/feed/", "http://coder-log.blogspot.com/feeds/posts/default",
        //                    "http://dev30.pl/feed/", "https://chrisseroka.wordpress.com/feed/",
        //                    "http://www.andrzejdubaj.com/feed/", "http://agatamroz.com.pl/feed/",
        //                    "https://fadwick.wordpress.com/feed/", "http://t-code.pl/atom.xml",
        //                    "http://zelazowy.github.io/feed.xml", "http://www.owsiak.org/?feed=rss2",
        //                    "http://programuje.net/feed/", "http://tomaszsokol.pl/feed/", "http://newstech.pl/feed/",
        //                    "http://findfriendsswift.blogspot.com/feeds/posts/default", "http://lazarusdev.pl/feed/",
        //                    "http://novakov.github.io/feed.xml", "http://tsovek.blogspot.com/feeds/posts/default",
        //                    "http://blog.buczaj.com/feed/", "http://piotrgankiewicz.com/feed/",
        //                    "https://admincenterblog.wordpress.com/feed/", "https://gettoknowthebob.wordpress.com/feed/",
        //                    "http://zszywacz.azurewebsites.net/feed/",
        //                    "http://improsoft.blogspot.com/feeds/posts/default", "http://lazybitch.com/feed",
        //                    "http://pewudev.pl/feed/", "http://mborowy.com/feed/",
        //                    "http://cleancodestruggle.blogspot.com/feeds/posts/default",
        //                    "http://lukaszmocarski.com/feed/", "http://blog.rakaz.pl/feed/",
        //                    "http://ionicdsp.eu/?feed=rss2", "http://www.codiferent.pl/feed/",
        //                    "http://spine.angrybits.pl/?feed=rss2", "http://bnowakowski.pl/en/feed/",
        //                    "http://kazaarblog.blogspot.com/feeds/posts/default",
        //                    "http://blog.yotkaz.me/feeds/posts/default", "http://langusblog.pl/index.php/feed/",
        //                    "http://podziemiazamkul.blogspot.com/feeds/posts/default",
        //                    "http://www.wlangiewicz.com/feed/", "http://resumees.net/devblog/feed/",
        //                    "https://uwagababaprogramuje.wordpress.com/feed/",
        //                    "http://terianil.blogspot.com/feeds/posts/default", "http://sirdigital.pl/index.php/feed/",
        //                    "http://mmalczewski.pl/index.php/feed/", "http://cojakodze.pl/feed/",
        //                    "http://mieczyk.vilya.pl/feed/", "https://jendaapkatygodniowo.wordpress.com/feed/",
        //                    "http://dziury-w-calym.pl/feed/", "http://sweetprogramming.com/feed/",
        //                    "https://branegblog.wordpress.com/feed/", "http://przemek.ciacka.com/feed.xml",
        //                    "http://maciektalaska.github.io/atom.xml", "http://www.mguzdek.pl/feed/",
        //                    "http://dspprojekt.blogspot.com/feeds/posts/default",
        //                    "https://tomaszprasolek.wordpress.com/feed/", "http://mrojecki.azurewebsites.net/rss",
        //                    "http://itka4yk.blogspot.com/feeds/posts/default", "http://whitebear.com.pl/feed/",
        //                    "http://mzieba.com/feed/", "https://alpac4blog.wordpress.com/feed/",
        //                    "http://mplonski.prv.pl/feed/", "http://blog.forigi.com/feed/",
        //                    "http://www.code-addict.pl/feed", "http://tomaszkacmajor.pl/index.php/feed/",
        //                    "http://marcinkruszynski.blogspot.com/feeds/posts/default", "http://blog.stanaszek.pl/feed/",
        //                    "http://memforis.info/feed/"
        //                };

        //    var rssChannels = x.Select(b => new RssChannel(b, b)).ToList();
        //    this.database.RssChannels.AddRange(rssChannels);
        //    this.database.SaveChanges();
        //}

        public bool CanRegisterWithinLimits()
        {
            var applicationConfiguration = this.database.ApplicationConfiguration.Single();
            return applicationConfiguration.UsersLimit > this.database.Users.Count();
        }

        public void ChangeApplicationRegistration(RegistrationSupported dtoStatus)
        {
            this.database.ApplicationConfiguration.Single().RegistrationSupported = dtoStatus;
            this.database.SaveChanges();
        }

        public void ChangeDisplayName(long currentUserId, string displayname)
        {
            var single = this.database.Users.Single(u => u.Id == currentUserId);
            single.DisplayName = displayname;
            this.database.SaveChanges();
        }

        public void ChangeEmail(long currentUserId, string email)
        {
            var single = this.database.Users.Single(u => u.Id == currentUserId);
            single.Email = email;
            this.database.SaveChanges();
        }

        public void ChangeUserLimit(long dtoLimit)
        {
            this.database.ApplicationConfiguration.Single().UsersLimit = dtoLimit;
            this.database.SaveChanges();
        }

        public void CopyAllUnreadElementsToUser(long currentUserId)
        {
            var observedUsersId = this.database.UsersSubscriptions.Where(x => x.FollowerId == currentUserId).ToList();
            var currentUserObservedUsersIds = observedUsersId.Select(x => x.ObservedId).ToList();
            var lastReadTime = this.database.Users.Single(x => x.Id == currentUserId).LastReadTime;
            var eventRssUserInteractions = this.database.EventsRssUserInteraction.Where(x => currentUserObservedUsersIds.Contains(x.UserId)).Where(x => x.Created >= lastReadTime).ToList();
            foreach(var userInteraction in eventRssUserInteractions)
            {
                var userSubscriptionEntryToRead = new UserSubscriptionEntryToRead
                {
                    EventRssUserInteractionId = userInteraction.Id,
                    UserSubscriptionId = observedUsersId.Single(o => o.ObservedId == userInteraction.UserId).Id
                };
                this.database.UsersSubscriptionsToRead.Add(userSubscriptionEntryToRead);
            }
            this.database.SaveChanges();
        }

        public void CreateNewSocialLogin(
            string identifierValue,
            AuthenticationTypeProvider authenticationTypeProvider,
            long newUserId)
        {
            var socialLogin = new SocialLogin(identifierValue, authenticationTypeProvider, newUserId);
            this.database.SocialLogins.Add(socialLogin);
            this.database.SaveChanges();
        }

        public void CreateNewSubscription(long followerId, long observedId)
        {
            if(this.IsUserSubscribedToUser(followerId, observedId))
            {
                return;
            }

            var userSubscription = new UserSubscription { FollowerId = followerId, ObservedId = observedId };
            this.database.UsersSubscriptions.Add(userSubscription);
            this.database.SaveChanges();
        }

        public void CreateNewSubscriptionForUserAndChannel(long userId, long channelId)
        {
            var channelTitle =
                this.database.RssChannels.Where(channel => channel.Id == channelId)
                    .Select(channel => channel.Title)
                    .Single();

            var rssChannelSubscription = new RssChannelSubscription(channelId, userId, channelTitle);
            this.database.RssChannelsSubscriptions.Add(rssChannelSubscription);
            this.database.SaveChanges();
        }

        public long CreateNewUser(string name, string email)
        {
            var user = new User { DisplayName = name, Email = email };

            this.database.Users.Add(user);
            this.database.SaveChanges();
            return user.Id;
        }

        public void DeleteSubscriptionFromUser(long channelId, long userId)
        {
            var channelSubscription =
                this.database.RssChannelsSubscriptions.Where(subscription => subscription.RssChannelId == channelId)
                    .Where(subscription => subscription.UserId == userId)
                    .Single();
            this.database.RssChannelsSubscriptions.Remove(channelSubscription);
            this.database.SaveChanges();
        }

        public void DeleteUserSubscription(long followerId, long observedId)
        {
            if(this.IsUserSubscribedToUser(followerId, observedId) == false)
            {
                return;
            }

            var userSubscription =
                this.database.UsersSubscriptions.Single(x => x.FollowerId == followerId && x.ObservedId == observedId);
            this.database.UsersSubscriptions.Remove(userSubscription);
            this.database.SaveChanges();
        }

        public bool DoesUserOwnsChannelSubscription(long subscriptionId, long currentUserId)
        {
            return this.database.RssChannelsSubscriptions.Any(x => x.UserId == currentUserId && x.Id == subscriptionId);
        }

        public bool DoesUserOwnsUserSubscription(long subscriptionId, long currentUserId)
        {
            var subscription =
                this.database.UsersSubscriptions.Where(x => x.Id == subscriptionId)
                    .SingleOrDefault(x => x.FollowerId == currentUserId);
            return subscription != null;
        }

        public SocialLogin FindSocialLogin(string socialLoginId, AuthenticationTypeProvider provider)
        {
            var socialLogin =
                this.database.SocialLogins.Where(login => login.SocialId == socialLoginId)
                    .Where(login => login.Provider == provider)
                    .SingleOrDefault();

            return socialLogin;
        }

        public long FindSubscriptionIdOfUserAndOfChannel(long userId, long channelId)
        {
            var channelSubscription =
                this.database.RssChannelsSubscriptions.Where(subscription => subscription.RssChannelId == channelId)
                    .Where(subscription => subscription.UserId == userId)
                    .Select(subscription => subscription.Id)
                    .SingleOrDefault();
            return channelSubscription;
        }

        public List<User> GetAllUsers()
        {
            return this.database.Users.ToList();
        }

        public List<long> GetChannelIdSubscriptionsForUser(long currentUserId)
        {
            var rssChannelSubscriptions =
                this.database.RssChannelsSubscriptions.Where(x => x.UserId == currentUserId)
                    .Select(x => x.RssChannelId)
                    .ToList();
            return rssChannelSubscriptions;
        }

        public RegistrationSupported GetCurrentRegistrationStatus()
        {
            return this.database.ApplicationConfiguration.Single().RegistrationSupported;
        }

        public List<long> GetIdByChannelUrl(List<string> urlstoChannels)
        {
            var longs =
                this.database.RssChannels.Where(channel => urlstoChannels.Contains(channel.Url))
                    .Select(channel => channel.Id)
                    .ToList();

            return longs;
        }

        public DateTime GetLatestUpdateDate(long rssChannelId)
        {
            var eventRssChannelUpdated =
                this.database.RssChannelUpdates.Where(c => c.RssChannelId == rssChannelId)
                    .OrderByDescending(c => c.Created)
                    .FirstOrDefault();
            return eventRssChannelUpdated?.Created ?? DateTime.MinValue;
        }

        public long GetNumberOfRegisteredUsers()
        {
            return this.database.Users.Count();
        }

        public long GetNumberOfRssNews()
        {
            return this.database.RssEntries.Count();
        }

        public long GetNumberOfRssSubscriptions()
        {
            return this.database.RssChannelsSubscriptions.Count();
        }

        public List<UserRole> GetRolesForUser(long currentUserId)
        {
            return this.database.UserRoles.Where(r => r.UserId == currentUserId).ToList();
        }

        public List<ItanRole> GetRolesTypesForUser(long currentUserId)
        {
            return this.database.UserRoles.Where(r => r.UserId == currentUserId).Select(r => r.RoleType).ToList();
        }

        public long GetUserId(string currentUserSocialLoginId, AuthenticationTypeProvider currentUserLoginProvider)
        {
            var socialLogin =
                this.database.SocialLogins.Where(login => login.SocialId == currentUserSocialLoginId)
                    .Where(login => login.Provider == currentUserLoginProvider)
                    .Select(login => login.UserId)
                    .Single();
            return socialLogin;
        }

        public DateTime GetUserLastReadTime(long userId)
        {
            var single = this.database.Users.Single(user => user.Id == userId);
            return single.LastReadTime;
        }

        public UserPrivateProfileDto GetUserPrivateDetails(long currentUserId)
        {
            var user =
                this.database.Users.Include(u => u.SocialLogins)
                    .Where(u => u.Id == currentUserId)
                    .ProjectTo<UserPrivateProfileDto>()
                    .Single();
            return user;
        }

        public void InsertReadRssToRead(long userId, long rssId, long dtoSubscriptionId)
        {
            var rssEntryToRead = new RssEntryToRead
            {
                IsRead = true,
                RssEntryId = rssId,
                RssChannelSubscriptionId = dtoSubscriptionId
            };

            this.database.RssEntriesToRead.Add(rssEntryToRead);
            this.database.SaveChanges();
        }

        public bool IsUserSubscribedToChannelId(long currentUserId, long channelId)
        {
            var any =
                this.database.RssChannelsSubscriptions.Any(
                    x => x.UserId == currentUserId && x.RssChannelId == channelId);
            return any;
        }

        public bool IsUserSubscribedToChannelUrl(long currentUserId, string rssChannelLink)
        {
            var any =
                this.database.RssChannelsSubscriptions.Where(x => x.UserId == currentUserId)
                    .Include(x => x.RssChannel)
                    .Where(x => x.RssChannel.Url == rssChannelLink)
                    .Any();
            return any;
        }

        public bool IsUserSubscribedToUser(long followerId, long observedId)
        {
            var isSubscribed =
                this.database.UsersSubscriptions.Any(s => s.FollowerId == followerId && s.ObservedId == observedId);
            return isSubscribed;
        }

        public List<RssChannel> LoadAllChannels()
        {
            return this.database.RssChannels.ToList();
        }

        public List<RssChannel> LoadAllChannelsForUser(long userIdToLoad)
        {
            var rssChannels =
                this.database.Users.Where(user => user.Id == userIdToLoad)
                    .Include(user => user.RssSubscriptionList)
                    .Include(user => user.RssSubscriptionList.Select(rsl => rsl.RssChannel))
                    .Single()
                    .RssSubscriptionList.Select(rsl => rsl.RssChannel)
                    .ToList();
            return rssChannels;
        }

        public List<RssChannelSubscriptionWithStatisticsData> LoadAllChannelsWithStatistics(long currentUserId, int skip, int take)
        {
            var channels = this.database
                               .RssChannels
                               .OrderBy(rssChannel => rssChannel.Id)
                               .Skip(skip)
                               .Take(take)
                               .Include(r => r.RssEntries)
                               .Include(r => r.Subscriptions);
            var rssChannelSubscriptionWithStatisticsDatas = channels
                    .Select(c => new RssChannelSubscriptionWithStatisticsData
                    {
                        Created = c.Created,
                        Id = c.Id,
                        Title = c.Title,
                        RssEntriesCount = c.RssEntries.Count,
                        IsSubscribed = c.Subscriptions.Select(s => s.UserId).Contains(currentUserId)
                    })
                    .ToList();
            return rssChannelSubscriptionWithStatisticsDatas;
        }

        public List<RssEntryToReadDTO> LoadAllEntriesFromChannelSubscription(long subscriptionId)
        {
            var rssEntryToReads =
                this.database.RssEntriesToRead.Where(r => r.RssChannelSubscriptionId == subscriptionId)
                    .Include(r => r.RssEntry)
                    .ProjectTo<RssEntryToReadDTO>()
                    .ToList();

            return rssEntryToReads;
        }

        public List<RssEntryToReadDTO> LoadAllEntriesFromSubscription(long subscriptionId)
        {
            var rssEntryToReads =
                this.database.RssEntriesToRead.Where(r => r.RssChannelSubscriptionId == subscriptionId)
                    .Include(r => r.RssEntry)
                    .ProjectTo<RssEntryToReadDTO>()
                    .ToList();

            return rssEntryToReads;
        }

        public List<UserSubscriptionEntryToReadDTO> LoadAllEntriesFromUserSubscription(long subscriptionId)
        {
            var userSubscriptions =
                this.database.UsersSubscriptions.Where(s => s.Id == subscriptionId)
                    .Include(s => s.EntriesToRead)
                    .SelectMany(s => s.EntriesToRead)
                    .Include(s => s.EventRssUserInteraction)
                    .Include(s => s.EventRssUserInteraction.RssEntry)
                    .ProjectTo<UserSubscriptionEntryToReadDTO>()
                    .ToList();

            return userSubscriptions.ToList();
        }

        public List<RssChannelSubscriptionDTO> LoadAllSubscriptionsForUser(long currentUserId)
        {
            string sql = "SELECT RCS.Title,\n"
              + "       RCS.Id,\n"
              + "       COUNT(*) AS 'Count'\n"
              + "FROM dbo.RssChannelSubscriptions AS rcs\n"
              + "     JOIN\n"
              + "(\n"
              + "    SELECT re.*\n"
              + "    FROM dbo.RssEntries AS re\n"
              + "         LEFT JOIN\n"
              + "    (\n"
              + "        SELECT *\n"
              + "        FROM dbo.RssEntriesToRead AS retr\n"
              + "        WHERE retr.RssChannelSubscriptionId IN\n"
              + "        (\n"
              + "            SELECT rcs.id\n"
              + "            FROM dbo.RssChannelSubscriptions AS rcs\n"
              + $"            WHERE rcs.UserId = {currentUserId}\n"
              + "        )\n"
              + "    ) AS RRR ON RRR.RssEntryId = re.id\n"
              + "    WHERE re.RssChannelId IN\n"
              + "    (\n"
              + "        SELECT rcs.RssChannelId\n"
              + "        FROM dbo.RssChannelSubscriptions AS rcs\n"
              + $"        WHERE rcs.UserId = {currentUserId}\n"
              + "    )\n"
              + "          AND RRR.id IS NULL\n"
              + ") AS RRE ON rcs.RssChannelId = RRE.RssChannelId\n"
              + $"WHERE rcs.UserId={currentUserId}"
              + "GROUP BY rcs.Id,\n"
              + "         rcs.Title;";

            var rssChannelSubscriptionDtos =
                this.database.Database.SqlQuery<RssChannelSubscriptionDTO>(sql).ToList();
            return rssChannelSubscriptionDtos;
        }

        public List<RssChannelSubscriptionDTO> LoadAllSubscriptionsForUser_Old(long currentUserId)
        {
            var channelSubscriptions = from subs in this.database.RssChannelsSubscriptions
                                       join rs in this.database.RssEntriesToRead on subs.Id equals
                                       rs.RssChannelSubscriptionId into rss
                                       join channel in this.database.RssChannels on subs.RssChannelId equals channel.Id
                                       into channels
                                       where subs.UserId == currentUserId
                                       select
                                       new RssChannelSubscriptionDTO
                                       {
                                           Id = subs.Id,
                                           Count = rss.Count(r => r.IsRead == false),
                                           Title = channels.FirstOrDefault().Title,
                                           RssChannelId = subs.RssChannelId,
                                           ChannelUrl = channels.FirstOrDefault().Url
                                       };

            return channelSubscriptions.ToList();
        }

        public List<RssChannelSubscription> LoadAllSubscriptionsWithRssEntriesToReadForUser(long currentUserId)
        {
            var rssChannelSubscriptions =
                this.database.RssChannelsSubscriptions.Where(x => x.UserId == currentUserId)
                    .Include(x => x.RssEntriesToRead)
                    .Include(x => x.RssEntriesToRead.Select(c => c.RssEntry))
                    .ToList();

            return rssChannelSubscriptions;
        }

        public List<RssEntryToReadDTO> LoadAllUnreadEntriesFromChannelSubscription(long subscriptionId)
        {
            var rssEntryToReads = from rssToRead in this.database.RssEntriesToRead
                                  join rss in this.database.RssEntries on rssToRead.RssEntryId equals rss.Id
                                  where
                                  rssToRead.RssChannelSubscriptionId == subscriptionId && rssToRead.IsRead == false
                                  select
                                  new RssEntryToReadDTO
                                  {
                                      Id = rssToRead.Id,
                                      IsRead = false,
                                      RssEntryDto =
                                              new RssEntryDTO
                                              {
                                                  Id = rss.Id,
                                                  PreviewText = rss.PreviewText,
                                                  Url = rss.Url,
                                                  Title = rss.Title,
                                                  PublicationDate =
                                                          rss.PublicationDate
                                              }
                                  };

            var list = rssEntryToReads.ToList();

            return list;
        }

        public List<RssEntryToReadDTO> LoadAllUnreadEntriesFromSubscription(long subscriptionId)
        {
            var rssEntryToReads = from rssToRead in this.database.RssEntriesToRead
                                  join rss in this.database.RssEntries on rssToRead.RssEntryId equals rss.Id
                                  where
                                  rssToRead.RssChannelSubscriptionId == subscriptionId && rssToRead.IsRead == false
                                  select
                                  new RssEntryToReadDTO
                                  {
                                      Id = rssToRead.Id,
                                      IsRead = false,
                                      RssEntryDto =
                                              new RssEntryDTO
                                              {
                                                  Id = rss.Id,
                                                  PreviewText = rss.PreviewText,
                                                  Url = rss.Url,
                                                  Title = rss.Title,
                                                  PublicationDate =
                                                          rss.PublicationDate
                                              }
                                  };

            var list = rssEntryToReads.ToList();

            return list;
        }

        public List<UserSubscriptionEntryToReadDTO> LoadAllUnreadEntriesFromUserSubscription(long subscriptionId)
        {
            var userSubscriptions =
                this.database.UsersSubscriptions.Where(s => s.Id == subscriptionId)
                    .Include(s => s.EntriesToRead)
                    .SelectMany(s => s.EntriesToRead)
                    .Include(s => s.EventRssUserInteraction)
                    .Include(s => s.EventRssUserInteraction.RssEntry)
                    .Where(s => !s.IsRead)
                    .ProjectTo<UserSubscriptionEntryToReadDTO>()
                    .ToList();

            return userSubscriptions.ToList();
        }

        public List<UserSubscriptionEntryToReadDTO> LoadAllUserEntriesFromSubscription(long subscriptionId)
        {
            var x = "select ERUI.Id as EruiId, ERUI.InteractionType, RE.* " +
                     "from UserSubscriptions US join EventRssUserInteractions ERUI on US.ObservedId = ERUI.UserId " +
                     "join RssEntries RE on ERUI.RssEntryId = RE.Id " +
                     $"where US.Id = {subscriptionId} AND " +
                     $"ERUI.InteractionType in ({(int)InteractionType.Clicked}, {(int)InteractionType.Navigated}) ";

            var rssEntryDtos = this.database.Database.SqlQuery<RssEntryDTO>(x).ToList();
            var userSubscriptionEntryToReadDtos = rssEntryDtos.Select(a => new UserSubscriptionEntryToReadDTO { RssEntryDto = a, Id = a.Id, IsRead = false }).ToList();
            return userSubscriptionEntryToReadDtos;
        }

        public List<UserPublicProfile> LoadAllUsersPublicProfileWithChannelsCount()
        {
            var userPublicProfiles = from U in this.database.Users
                                     join S in this.database.RssChannelsSubscriptions on U.Id equals S.UserId
                                     select
                                     new UserPublicProfile
                                     {
                                         Id = U.Id,
                                         DisplayName = U.DisplayName,
                                         ChannelsCount = U.RssSubscriptionList.Count
                                     };

            return userPublicProfiles.Distinct().ToList();
        }

        public List<UserSubscriptionEntryToReadDTO> LoadAllUserUnreadEntriesFromSubscription(long subscriptionId)
        {
            var x = "SELECT RE.Title,RE.PublicationDate, RE.PreviewText, RE.Url, ERUI.Id " +
                    "from UserSubscriptions US join EventRssUserInteractions ERUI on US.ObservedId = ERUI.UserId " +
                    "join RssEntries RE on ERUI.RssEntryId = RE.Id " +
                    "LEFT JOIN UserSubscriptionEntryToReads USETR on ERUI.Id = USETR.EventRssUserInteractionId " +
                    $"where US.Id = {subscriptionId} AND USETR.Id is NULL AND " +
                    $"ERUI.InteractionType in ({(int)InteractionType.Clicked}, {(int)InteractionType.Navigated}) ";

            var rssEntryDtos = this.database.Database.SqlQuery<RssEntryDTO>(x).ToList();
            var userSubscriptionEntryToReadDtos = rssEntryDtos.Select(a => new UserSubscriptionEntryToReadDTO { RssEntryDto = a, Id = a.Id, IsRead = false }).ToList();
            return userSubscriptionEntryToReadDtos;
        }

        public ApplicationConfigurationDTO LoadApplicationConfiguration()
        {
            var applicationConfiguration = this.database.ApplicationConfiguration.Single();
            var dto = this.mapper.Map<ApplicationConfiguration, ApplicationConfigurationDTO>(applicationConfiguration);
            return dto;
        }

        public RssChannelInformationDTO LoadChannelChannelInformation(long subscriptionId)
        {
            var rssChannelSubscription =
                this.database.RssChannelsSubscriptions.Include(x => x.RssChannel)
                    .Where(x => x.Id == subscriptionId)
                    .ProjectTo<RssChannelInformationDTO>()
                    .Single();

            return rssChannelSubscription;
        }

        public RssChannelForUpdateDTO LoadChannelToUpdate()
        {
            var sqlQuery = @"select top 1 c.Id,c.Url,
case when(u.c is null)then convert(datetime2, '0001-01-01 00:00:00.0000000', 121) else u.c end as Updated
from RssChannels c
left join 
(select u.RssChannelId, max(u.Created) c from EventRssChannelUpdates u
group by u.RssChannelId) u
on c.Id=u.RssChannelId
order by Updated";

            var rssChannelForUpdateDto = this.database.Database.SqlQuery<RssChannelForUpdateDTO>(sqlQuery);

            return rssChannelForUpdateDto.Single();
        }

        public RssChannelExceptions LoadExceptionEventsCount(long currentUserId)
        {
            string sql = "SELECT count(*) AS 'Count'\n"
                         + "FROM dbo.ItanExceptions ie\n"
                         + "WHERE ie.Id NOT IN\n"
                         + "(\n"
                         + "    SELECT ietr.ItanExceptionId\n"
                         + "    FROM dbo.ItanExceptionsToRead ietr\n"
                         + $"    WHERE userid = {currentUserId}\n"
                         + ")";
            var c = this.database.Database.SqlQuery<RssChannelExceptions>(sql).Single();
            return c;
        }

        public List<ExceptionEventDto> LoadExceptionList(long userId)
        {
            string sql = "SELECT *\n"
                         + "FROM dbo.ItanExceptions ie\n"
                         + "WHERE ie.Id NOT IN\n"
                         + "(\n"
                         + "    SELECT ietr.ItanExceptionId\n"
                         + "    FROM dbo.ItanExceptionsToRead ietr\n"
                         + $"    WHERE userid = {userId}\n"
                         + ")\n"
                         + "ORDER BY ie.Created;";
            var exceptionEventDtos = this.database.Database.SqlQuery<ExceptionEventDto>(sql)
                .ToList();
            return exceptionEventDtos;
        }

        public List<NameAndCountUserSubscription> LoadNameAndCountForUser(long currentUserId)
        {
            var x =
                "select USS.Id as SubscriptionId, A.* from UserSubscriptions USS " +
                "join(select U.Id as 'UserId', U.DisplayName, Count(*) as 'Count' " +
                "from UserSubscriptions US join EventRssUserInteractions ERUI on US.ObservedId = ERUI.UserId " +
                "join Users U on US.ObservedId = U.Id left join UserSubscriptionEntryToReads USETR " +
                $"on ERUI.Id = USETR.EventRssUserInteractionId where FollowerId = {currentUserId} AND ERUI.InteractionType IN({(int)InteractionType.Clicked}, {(int)InteractionType.Navigated}) " +
                "AND USETR.Id is null GROUP BY U.Id, U.DisplayName) A on USS.ObservedId = A.UserId ";

            var nameAndCountUserSubscriptions =
                this.database.Database.SqlQuery<NameAndCountUserSubscription>(x).ToList();
            return nameAndCountUserSubscriptions;
        }

        public List<RssEntryDTO> LoadRss(long subscriptionId, long userId, int skip, int take)
        {
            string sql = "SELECT RE.Title,\n"
                         + "       RE.PublicationDate,\n"
                         + "       RE.PreviewText,\n"
                         + "       RE.Id,\n"
                         + "       RE.Url\n"
                         + "FROM dbo.RssEntries AS re\n"
                         + "     JOIN dbo.RssChannelSubscriptions AS rcs ON re.RssChannelId = rcs.RssChannelId\n"
                         + "WHERE re.Id NOT IN\n"
                         + "(\n"
                         + "    SELECT retr.RssEntryId\n"
                         + "    FROM dbo.RssChannelSubscriptions AS rcs\n"
                         + "         JOIN dbo.RssEntriesToRead AS retr ON retr.RssChannelSubscriptionId = rcs.Id\n"
                         + $"    WHERE rcs.UserId = {userId}\n"
                         + $"          AND rcs.id = {subscriptionId}\n"
                         + ")\n"
                         + $"    AND rcs.id = {subscriptionId}\n"
                         + "     ORDER BY RE.Id\n"
                         + $"    OFFSET {skip} row\n"
                         + $"    FETCH NEXT {take} rows only\n";

            var rssEntryToReadDtos = this.database.Database.SqlQuery<RssEntryDTO>(sql).ToList();
            return rssEntryToReadDtos;
        }

        public RssChannelDTO LoadRssChannel(long id)
        {
            var rssChannel = from c in this.database.RssChannels
                             join e in this.database.RssEntries on c.Id equals e.RssChannelId into e
                             join u in this.database.RssChannelUpdates on c.Id equals u.RssChannelId into u
                             where c.Id == id
                             select
                             new RssChannelDTO
                             {
                                 Added = c.Created,
                                 ChannelId = c.Id,
                                 Title = c.Title,
                                 Updated =
                                         u.OrderByDescending(o => o.Created).FirstOrDefault().Created,
                                 Entries =
                                         e.Select(
                                             s =>
                                                 new RssEntryDTO
                                                 {
                                                     Id = s.Id,
                                                     Title = s.Title,
                                                     PreviewText = s.PreviewText,
                                                     PublicationDate = s.Created,
                                                     Url = s.Url
                                                 }).Distinct().ToList()
                             };

            return rssChannel.Single();
        }

        public RssChannelUpdateds LoadUpdateEventsCount(long currentUserId)
        {
            string sql = "SELECT COUNT(*) as Count\n"
                         + "FROM dbo.EventRssChannelUpdates ercu\n"
                         + "WHERE ercu.Id NOT IN\n"
                         + "(\n"
                         + "    SELECT dbo.EventRssChannelUpdateToRead.EventRssChannelUpdatedId\n"
                         + "    FROM EventRssChannelUpdateToRead\n"
                         + $"    WHERE Userid = {currentUserId}\n"
                         + ");";
            var rssChannelUpdateds = this.database.Database.SqlQuery<RssChannelUpdateds>(sql)
                                         .Single();
            return rssChannelUpdateds;
        }

        public void MarkChannelUpdateClicked(long cui, long id)
        {
            var eventRssChannelUpdatedToRead = new EventRssChannelUpdatedToRead
            {
                EventRssChannelUpdatedId = id,
                IsViewed = true,
                UserId = cui
            };
            this.database.EventRssChannelUpdatedsToRead.Add(eventRssChannelUpdatedToRead);
            this.database.SaveChanges();
        }

        public void MarkChannelUpdateSkipped(long cui, List<long> entries)
        {
            var eventRssChannelUpdatedToReads = entries.Select(e =>
                                                                   new EventRssChannelUpdatedToRead
                                                                   {
                                                                       EventRssChannelUpdatedId = e,
                                                                       IsSkipped = true,
                                                                       UserId = cui
                                                                   });
            this.database.EventRssChannelUpdatedsToRead.AddRange(eventRssChannelUpdatedToReads);
            this.database.SaveChanges();
        }

        public void MarkChannelCreateClicked(long cui, long id)
        {
            var eventRssChannelUpdatedToRead = new EventRssChannelCreatedToRead
            {
                EventRssChannelCreatedId = id,
                IsViewed = true,
                UserId = cui
            };
            this.database.EventRssChannelCreatedToRead.Add(eventRssChannelUpdatedToRead);
            this.database.SaveChanges();
        }

        public void MarkChannelCreateSkipped(long cui, List<long> entries)
        {
            var eventRssChannelUpdatedToReads = entries.Select(e =>
                                                                   new EventRssChannelCreatedToRead
                                                                   {
                                                                       EventRssChannelCreatedId = e,
                                                                       IsSkipped = true,
                                                                       UserId = cui
                                                                   });
            this.database.EventRssChannelCreatedToRead.AddRange(eventRssChannelUpdatedToReads);
            this.database.SaveChanges();
        }

        public FeedEntries GetFeedEntries(long feedId, long skip, long take)
        {
            var rssEntryDtos = this.database.RssEntries.Where(e => e.RssChannelId == feedId)
                .OrderBy(e => e.PublicationDate).Skip((int)skip).Take((int)take).Select(
                    e => new RssEntryDTO
                             {
                                 Id = e.Id,
                                 PreviewText = e.PreviewText,
                                 PublicationDate = e.Created,
                                 Title = e.Title,
                                 Url = e.Url
                             }).ToList();
            var feedEntries = new FeedEntries(rssEntryDtos);
            return feedEntries;
        }

        public NumberOfRssFeeds ReadNumberOfAllRssFeeds()
        {
            var count = this.database.RssChannels.Count();
            return new NumberOfRssFeeds(count);
        }

        public List<ChannelUpdateEventDto> LoadUpdateEvents(long userId)
        {
            string sql = "SELECT ercu.Id,ercu.Created as 'Updated',rc.Title as 'ChannelTitle'\n"
                         + "FROM dbo.EventRssChannelUpdates ercu\n"
                         + "JOIN dbo.RssChannels rc ON ercu.RssChannelId = rc.Id\n"
                         + "WHERE ercu.Id NOT IN\n"
                         + "(\n"
                         + "    SELECT dbo.EventRssChannelUpdateToRead.EventRssChannelUpdatedId\n"
                         + "    FROM EventRssChannelUpdateToRead\n"
                         + $"    WHERE Userid = {userId}\n"
                         + ");";

            var channelUpdateEventDtos = this.database.Database.SqlQuery<ChannelUpdateEventDto>(sql).ToList();
            return channelUpdateEventDtos;
        }

        public List<ChannelCreateEventDto> LoadCreationEvents(long userId)
        {
            string sql = "SELECT ercc.Id, ercc.Created as 'Updated',rc.Title as 'ChannelTitle'\n"
                         + "FROM dbo.EventRssChannelCreateds ercc\n"
                         + "JOIN dbo.RssChannels rc ON ercc.RssChannelId = rc.Id\n"
                         + "WHERE ercc.Id NOT IN\n"
                         + "(\n"
                         + "    SELECT dbo.EventRssChannelCreatedsToRead.EventRssChannelCreatedId\n"
                         + "    FROM EventRssChannelCreatedsToRead\n"
                         + $"    WHERE UserId = {userId}\n"
                         + ");";

            var channelUpdateEventDtos = this.database.Database.SqlQuery<ChannelCreateEventDto>(sql).ToList();
            return channelUpdateEventDtos;
        }

        public ChannelUrlAndTitleDTO LoadUrlAndTitle(long channelId)
        {
            var project =
                this.database.RssChannels.Where(x => x.Id == channelId).ProjectTo<ChannelUrlAndTitleDTO>().Single();
            return project;
        }

        public List<string> LoadUrlsForAllChannels()
        {
            return
                this.database.RssChannels.Select(channel => channel.Url)
                    .ToList()
                    .Select(x => x.ToLowerInvariant())
                    .ToList();
        }

        public RssChannelInformationDTO LoadUserChannelInformation(long subscriptionId)
        {
            var userSubscription =
                this.database
                    .UsersSubscriptions
                    .Include(x => x.Observed)
                    .Where(x => x.Id == subscriptionId)
                    .ProjectTo<RssChannelInformationDTO>()
                    .Single();

            return userSubscription;
        }

        public UserPublicProfileDto LoadUserPublicProfile(long id)
        {
            var users =
                from u in
                this.database.Users.Include(x => x.RssSubscriptionList)
                    .Include(x => x.RssSubscriptionList.Select(xx => xx.RssChannel))
                    .Include(x => x.EventsRssViewed)
                    .Include(x => x.EventsRssViewed.Select(xx => xx.RssEntry))
                where u.Id == id
                select
                new UserPublicProfileDto
                {
                    Id = u.Id,
                    Channels = u.RssSubscriptionList.Select(x => x.Title).ToList(),
                    DisplayName = u.DisplayName,
                    ChannelsCount = u.RssSubscriptionList.Count,
                    Events = u.EventsRssViewed.Select(
                                    e =>
                                        new EventRssUserInteractionDTO
                                        {
                                            Title = e.RssEntry.Title,
                                            RssId = e.RssEntryId,
                                            Viewed = e.Created
                                        })
                                .ToList(),
                };


            var d = users.Single();
            return d;
        }

        public void MarkAllReadForUserAndSubscription(long subscriptionId, List<long> id)
        {
            this.database.RssEntriesToRead.Where(r => r.RssChannelSubscriptionId == subscriptionId)
                .Where(r => id.Contains(r.Id))
                .Include(r => r.RssEntry)
                .ToList()
                .ForEach(r => r.IsRead = true);
            this.database.SaveChanges();
        }

        public void MarkChannelEntriesSkipped(long modelSubscriptionId, List<long> ids)
        {
            var rssEntryToReads =
                ids.Select(
                    x =>
                        new RssEntryToRead
                        {
                            IsSkipped = true,
                            RssChannelSubscriptionId = modelSubscriptionId,
                            RssEntryId = x,
                        });

            this.database.Configuration.ValidateOnSaveEnabled = false;
            foreach(var rssEntryToRead in rssEntryToReads)
            {
                this.database.RssEntriesToRead.Add(rssEntryToRead);
                this.database.SaveChanges();

            }

            this.database.Configuration.ValidateOnSaveEnabled = true;
        }

        public void MarkChannelRead(List<long> ids)
        {
            var formattableString = $"UPDATE RssEntriesToRead SET IsRead=1 WHERE Id in ({string.Join(",", ids)})";
            this.database.Database.ExecuteSqlCommand(formattableString);
        }

        public void MarkUserRead(List<long> ids)
        {
            this.database.UsersSubscriptionsToRead.Where(x => ids.Contains(x.Id)).ToList().ForEach(x => x.IsRead = true);
            this.database.SaveChanges();
        }

        public void SaveChannelCreatedEventToDatabase(long submitterId, long eventRssChannelCreated)
        {
            var rssChannelCreated = new EventRssChannelCreated { RssChannelId = eventRssChannelCreated, SubmitterId = submitterId };
            this.database.EventRssChannelCreated.Add(rssChannelCreated);
            this.database.SaveChanges();
        }

        public void SaveContactAdministrationEventEventToDatabase(long contactId)
        {
            var contactAdministrationEvent = new ContactAdministrationEvent { ContactAdministrationId = contactId };
            this.database.ContactsAdministrationEvents.Add(contactAdministrationEvent);
            this.database.SaveChanges();
        }

        public void SaveEvent(long eventRssChannelUpdated)
        {
            var rssChannelUpdated = new EventRssChannelUpdated { RssChannelId = eventRssChannelUpdated };
            this.database.RssChannelUpdates.Add(rssChannelUpdated);
            this.database.SaveChanges();
        }

        public void SaveToDatabase(SocialLogin socialLogin)
        {
            this.database.SocialLogins.Add(socialLogin);
            this.database.SaveChanges();
        }

        public void SaveToDatabase(List<NewRssEntryDTO> rssEntriesList)
        {
            var rssEntries = this.mapper.Map<List<NewRssEntryDTO>, List<RssEntry>>(rssEntriesList);
            this.database.RssEntries.AddRange(rssEntries);
            this.database.SaveChanges();
        }

        public void SaveToDatabase(List<RssChannelSubscription> rssChannelSubscriptions)
        {
            this.database.RssChannelsSubscriptions.AddRange(rssChannelSubscriptions);
            this.database.SaveChanges();
        }

        public void SaveToDatabase(IEnumerable<ItanException> exceptions)
        {
            this.database.ItanExceptions.AddRange(exceptions);
            this.database.SaveChanges();
        }

        public void SaveExceptionToDatabase(IEnumerable<EventItanException> events)
        {
            this.database.EventException.AddRange(events);
            this.database.SaveChanges();
        }

        public long SaveContactAdministrationToDatabase(ContactAdministrationDto entity)
        {
            var contactAdministration = new ContactAdministration
            {
                Email = entity.Email,
                Message = entity.Message,
                Name = entity.Name,
                Topic = entity.Topic
            };
            this.database.ContactsAdministration.Add(contactAdministration);
            this.database.SaveChanges();
            return contactAdministration.Id;
        }

        public void SaveToDatabase(long submitterId, List<RssSourceWithUrlAndTitle> channelsNewToGlobalSpace)
        {
            var rssChannels = channelsNewToGlobalSpace.Select(x => new RssChannel(x.Url, x.Title, submitterId));
            this.database.RssChannels.AddRange(rssChannels);
            this.database.SaveChanges();
        }

        public void Subscribe(long idByChannelUrl, long currentUserId)
        {
            var title = this.database.RssChannels.Single(x => x.Id == idByChannelUrl).Title;
            var rssChannelSubscription = new RssChannelSubscription(idByChannelUrl, currentUserId, title);
            this.database.RssChannelsSubscriptions.Add(rssChannelSubscription);
            this.database.SaveChanges();
        }

        public void Subscribe(long idByChannelUrl, long currentUserId, string channelIdRssChannelName)
        {
            var rssChannelSubscription = new RssChannelSubscription(
                idByChannelUrl,
                currentUserId,
                channelIdRssChannelName);
            this.database.RssChannelsSubscriptions.Add(rssChannelSubscription);
            this.database.SaveChanges();
        }

        public void UpdateDisplayNames(List<User> emptyDisplay)
        {
            var ids = emptyDisplay.Select(x => x.Id).ToList();
            var users = this.database.Users.Where(user => ids.Contains(user.Id)).ToList();
            emptyDisplay.ForEach(newname => users.Single(u => u.Id == newname.Id).DisplayName = newname.DisplayName);
            this.database.SaveChanges();
        }

        public void UpdateRssLastUpdateTimeToDatabase(List<long> rssChannels)
        {
            var channels =
                this.database.RssChannels.Include(x => x.Updates)
                    .Where(channel => rssChannels.Contains(channel.Id))
                    .ToList();

            channels.ForEach(
                c => { c.RssLastUpdatedTime = c.Updates.OrderByDescending(d => d.Created).First().Created; });
            this.database.SaveChanges();
        }

        public void UpdateUserLastReadTime(long currentUserId, DateTime now)
        {
            this.database.Users.Single(u => u.Id == currentUserId).LastReadTime = now;
            this.database.SaveChanges();
        }

        public bool UserIsRegistered(AuthenticationTypeProvider authenticationTypeProvider, string userId)
        {
            var exists =
                this.database.SocialLogins.Where(l => l.Provider == authenticationTypeProvider)
                    .Where(l => l.SocialId == userId)
                    .Any();
            return exists;
        }

        public void MarkPersonActivityClicked(long id, long subscriptionId)
        {
            var userSubscriptionEntryToRead = new UserSubscriptionEntryToRead
            {
                EventRssUserInteractionId = id,
                IsRead = true,
                UserSubscriptionId = subscriptionId
            };
            this.database.UsersSubscriptionsToRead.Add(userSubscriptionEntryToRead);
            this.database.SaveChanges();
        }

        public void AddEventPersonActivityClicked(long cui, long id)
        {
            var rssEntryId = this.database.EventsRssUserInteraction.Single(x => x.Id == id)
                                 .RssEntryId;
            var eventRssUserInteraction = new EventRssUserInteraction
            {
                InteractionType = InteractionType.Clicked,
                RssEntryId = rssEntryId,
                UserId = cui
            };
            this.database.EventsRssUserInteraction.Add(eventRssUserInteraction);
            this.database.SaveChanges();
        }

        public void MarkPersonActivityNavigated(long rssId, long subscriptionId)
        {
            this.database.UsersSubscriptionsToRead
                .Single(x => x.UserSubscriptionId == subscriptionId && x.EventRssUserInteractionId == rssId)
                .IsViewed = true;
            this.database.SaveChanges();
        }

        public void AddEventPersonActivityNavigated(long userId, long rssId)
        {
            var rssEntryId = this.database.EventsRssUserInteraction.Single(x => x.Id == rssId)
                                 .RssEntryId;
            var eventRssUserInteraction = new EventRssUserInteraction
            {
                InteractionType = InteractionType.Navigated,
                RssEntryId = rssEntryId,
                UserId = userId
            };
            this.database.EventsRssUserInteraction.Add(eventRssUserInteraction);
            this.database.SaveChanges();
        }

        public void MarkPersonActivitySkipped(long subscriptionId, List<long> entries)
        {
            var toSkip = entries.Select(
                    i =>
                        new UserSubscriptionEntryToRead
                        {
                            IsRead = false,
                            IsSkipped = true,
                            EventRssUserInteractionId = i,
                            UserSubscriptionId = subscriptionId
                        }).ToList();

            this.database.UsersSubscriptionsToRead.AddRange(toSkip);
            this.database.SaveChanges();
        }

        public void AddEventPersonActivitySkipped(long cui, List<long> entries)
        {
            var rssEntryId = (from xxx in this.database.EventsRssUserInteraction
                              where entries.Contains(xxx.Id)
                              select xxx.RssEntryId).ToList();

            var eventRssUserInteractions = rssEntryId.Select(ev => new EventRssUserInteraction
            {
                InteractionType = InteractionType.Skipped,
                RssEntryId = ev,
                UserId = cui
            });
            this.database.EventsRssUserInteraction.AddRange(eventRssUserInteractions);
            this.database.SaveChanges();
        }

        public RssChannelCreations LoadCreateEventsCount(long currentUserId)
        {
            string sql = "SELECT COUNT(*) as Count\n"
                         + "FROM dbo.EventRssChannelCreateds ercc\n"
                         + "WHERE ercc.Id NOT IN\n"
                         + "(\n"
                         + "    SELECT dbo.EventRssChannelCreatedsToRead.EventRssChannelCreatedId\n"
                         + "    FROM EventRssChannelCreatedsToRead\n"
                         + $"    WHERE UserId = {currentUserId}\n"
                         + ");";
            var rssChannelUpdateds = this.database.Database.SqlQuery<RssChannelCreations>(sql)
                                         .Single();
            return rssChannelUpdateds;
        }

        public void MarkExceptionActivityClicked(long cui, long id)
        {
            var itanExceptionsToRead = new ItanExceptionToRead
            {
                IsViewed = true,
                ItanExceptionId = id,
                UserId = cui
            };
            this.database.ItanExceptionsToRead.Add(itanExceptionsToRead);
            this.database.SaveChanges();
        }

        public void MarkExceptionActivitySkipped(long cui, List<long> entries)
        {
            var results = entries.Select(e =>
                                             new ItanExceptionToRead
                                             {
                                                 ItanExceptionId = e,
                                                 UserId = cui,
                                                 IsSkipped = true
                                             })
                                 .ToList();
            this.database.ItanExceptionsToRead.AddRange(results);
            this.database.SaveChanges();
        }

        private void SaveFeatureRequestToDatabase(FeatureRequest featureRequest)
        {
            this.database.FeatureRequests.Add(featureRequest);
            this.database.SaveChanges();
        }

        public void MarkPersonEntriesSkipped(long modelSubscriptionId, List<long> ids)
        {
            var toSkip = ids.Select(
                            i =>
                                new UserSubscriptionEntryToRead
                                {
                                    IsRead = false,
                                    IsSkipped = true,
                                    EventRssUserInteractionId = i,
                                    UserSubscriptionId = modelSubscriptionId
                                }).ToList();

            this.database.UsersSubscriptionsToRead.AddRange(toSkip);
            this.database.SaveChanges();
        }

        public void MarkRssEntriesSkipped(long subscriptionId, List<long> ids)
        {
            var toSkip = ids.Select(
                i =>
                    new RssEntryToRead
                    {
                        IsRead = false,
                        IsSkipped = true,
                        RssEntryId = i,
                        RssChannelSubscriptionId = subscriptionId
                    }).ToList();

            this.database.RssEntriesToRead.AddRange(toSkip);
            this.database.SaveChanges();
        }

        public long SaveToDatabase(ContactAdministrationDto entity)
        {
            var contactAdministration = new ContactAdministration
            {
                Email = entity.Email,
                Message = entity.Message,
                Name = entity.Name,
                Topic = entity.Topic
            };
            this.database.ContactsAdministration.Add(contactAdministration);
            this.database.SaveChanges();
            return contactAdministration.Id;
        }

        public void MarkRssClicked(long id, long subscriptionId)
        {
            var data = new RssEntryToRead
            {
                RssEntryId = id,
                RssChannelSubscriptionId = subscriptionId,
                IsRead = true
            };
            this.database.RssEntriesToRead.Add(data);
            this.database.SaveChanges();
        }
        public void AddEventRssClicked(long cui, long id)
        {
            var data = new EventRssUserInteraction
            {
                InteractionType = InteractionType.Clicked,
                RssEntryId = id,
                UserId = cui
            };
            this.database.EventsRssUserInteraction.Add(data);
            this.database.SaveChanges();
        }
        public void MarkRssNavigated(long rssId, long subscriptionId)
        {
            this.database.RssEntriesToRead
                .Single(x => x.RssEntryId == rssId && x.RssChannelSubscriptionId == subscriptionId)
                .IsViewed = true;
            this.database.SaveChanges();
        }
        public void AddEventRssNavigated(long userId, long rssId)
        {
            var data = new EventRssUserInteraction
            {
                InteractionType = InteractionType.Navigated,
                RssEntryId = rssId,
                UserId = userId
            };
            this.database.EventsRssUserInteraction.Add(data);
            this.database.SaveChanges();
        }
    }
}
