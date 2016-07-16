namespace IsThereAnyNews.DataAccess.Implementation
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;

    public class RssChannelsRepository : IRssChannelsRepository
    {
        private readonly ItanDatabaseContext database;

        public RssChannelsRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public List<RssChannel> AddToGlobalSpace(List<RssChannel> importFromUpload)
        {
            var rssChannels = importFromUpload.Select(channel => new RssChannel(channel.Url, channel.Title)).ToList();
            this.database.RssChannels.AddRange(rssChannels);
            this.database.SaveChanges();
            return rssChannels;
        }

        public List<RssChannelSubscriptionWithStatisticsData> LoadAllChannelsWithStatistics()
        {
            var f =
                from channel in this.database.RssChannels
                join subscription in this.database.RssChannelsSubscriptions
                    on channel.Id equals subscription.RssChannelId into RCS
                join entries in this.database.RssEntries
                    on channel.Id equals entries.RssChannelId into RE
                select
                    new RssChannelSubscriptionWithStatisticsData
                    {
                        Id = channel.Id,
                        Title = channel.Title,
                        SubscriptionsCount = RCS.Count(),
                        RssEntriesCount = RE.Count(),
                        Created = channel.Created,
                        Updated = channel.Updated
                    };

            return f.Distinct().ToList();
        }

        public RssChannel Load(long id)
        {
            return this.database.RssChannels.Single(channel => channel.Id == id);
        }

        public List<RssChannel> LoadAllChannelsForUser(long userIdToLoad)
        {
            var rssChannels = this.database
                .Users
                .Where(user => user.Id == userIdToLoad)
                .Include(user => user.RssSubscriptionList)
                .Include(user => user.RssSubscriptionList.Select(rsl => rsl.RssChannel))
                .Single()
                .RssSubscriptionList.Select(rsl => rsl.RssChannel)
                .ToList();
            return rssChannels;
        }

        public void SaveToDatabase(List<RssChannel> channelsNewToGlobalSpace)
        {
            this.database.RssChannels.AddRange(channelsNewToGlobalSpace);
            this.database.SaveChanges();
        }

        public List<long> GetIdByChannelUrl(List<string> urlstoChannels)
        {
            var longs = this.database.RssChannels
                .Where(channel => urlstoChannels.Contains(channel.Url))
                .Select(channel => channel.Id)
                .ToList();

            return longs;
        }

        public RssChannel LoadRssChannel(long id)
        {
            return this.database
                .RssChannels
                .Include(channel => channel.RssEntries)
                .Single(x => x.Id == id);
        }

        public void UpdateRssLastUpdateTimeToDatabase(List<RssChannel> rssChannels)
        {
            var ids = rssChannels.Select(x => x.Id).ToList();

            var channels = this.database.RssChannels.Where(channel => ids.Contains(channel.Id)).ToList();
            channels.ForEach(channel =>
            {
                channel.RssLastUpdatedTime = rssChannels
                    .Single(x => x.Id == channel.Id).RssLastUpdatedTime;
            });

            this.database.SaveChanges();
        }

        public void Blah()
        {
            var x = new[]
            {
                "http://feeds.reuters.com/news/artsculture",
                "http://feeds.reuters.com/reuters/businessNews",
                "http://feeds.reuters.com/reuters/companyNews",
                "http://feeds.reuters.com/reuters/entertainment",
                "http://feeds.reuters.com/reuters/environment",
                "http://feeds.reuters.com/reuters/healthNews",
                "http://feeds.reuters.com/reuters/lifestyle",
                "http://feeds.reuters.com/news/reutersmedia",
                "http://feeds.reuters.com/news/wealth",
                "http://feeds.reuters.com/reuters/MostRead",
                "http://feeds.reuters.com/reuters/oddlyEnoughNews",
                "http://feeds.reuters.com/ReutersPictures",
                "http://feeds.reuters.com/reuters/peopleNews",
                "http://feeds.reuters.com/Reuters/PoliticsNews",
                "http://feeds.reuters.com/reuters/scienceNews",
                "http://feeds.reuters.com/reuters/sportsNews",
                "http://feeds.reuters.com/reuters/technologyNews",
                "http://feeds.reuters.com/reuters/topNews",
                "http://feeds.reuters.com/Reuters/domesticNews",
                "http://feeds.reuters.com/Reuters/worldNews",
                "http://feeds.reuters.com/reuters/bankruptcyNews",
                "http://feeds.reuters.com/reuters/bondsNews",
                "http://feeds.reuters.com/news/deals",
                "http://feeds.reuters.com/news/economy",
                "http://feeds.reuters.com/reuters/globalmarketsNews",
                "http://feeds.reuters.com/news/hedgefunds",
                "http://feeds.reuters.com/reuters/hotStocksNews",
                "http://feeds.reuters.com/reuters/mergersNews",
                "http://feeds.reuters.com/reuters/governmentfilingsNews",
                "http://feeds.reuters.com/reuters/summitNews",
                "http://feeds.reuters.com/reuters/USdollarreportNews",
                "http://feeds.reuters.com/news/usmarkets",
                "http://feeds.reuters.com/reuters/basicmaterialsNews",
                "http://feeds.reuters.com/reuters/cyclicalconsumergoodsNews",
                "http://feeds.reuters.com/reuters/USenergyNews",
                "http://feeds.reuters.com/reuters/environment",
                "http://feeds.reuters.com/reuters/financialsNews",
                "http://feeds.reuters.com/reuters/UShealthcareNews",
                "http://feeds.reuters.com/reuters/industrialsNews",
                "http://feeds.reuters.com/reuters/USmediaDiversifiedNews",
                "http://feeds.reuters.com/reuters/noncyclicalconsumergoodsNews",
                "http://feeds.reuters.com/reuters/technologysectorNews",
                "http://feeds.reuters.com/reuters/utilitiesNews",
                "http://feeds.reuters.com/reuters/blogs/FinancialRegulatoryForum",
                "http://feeds.reuters.com/reuters/blogs/GlobalInvesting",
                "http://feeds.reuters.com/reuters/blogs/HugoDixon",
                "http://feeds.reuters.com/reuters/blogs/India",
                "http://feeds.reuters.com/reuters/blogs/JamesSaft",
                "http://feeds.reuters.com/reuters/blogs/macroscope",
                "http://feeds.reuters.com/reuters/blogs/mediafile",
                "http://feeds.reuters.com/reuters/blogs/newsmaker",
                "http://feeds.reuters.com/reuters/blogs/photo",
                "http://feeds.reuters.com/reuters/blogs/SummitNotebook",
                "http://feeds.reuters.com/reuters/blogs/talesfromthetrail",
                "http://feeds.reuters.com/reuters/blogs/the-great-debate",
                "http://feeds.reuters.com/UnstructuredFinance",
                "http://feeds.reuters.com/reuters/USVideoBreakingviews",
                "http://feeds.reuters.com/reuters/USVideoBusiness",
                "http://feeds.reuters.com/reuters/USVideoBusinessTravel",
                "http://feeds.reuters.com/reuters/USVideoChrystiaFreeland",
                "http://feeds.reuters.com/reuters/USVideoEntertainment",
                "http://feeds.reuters.com/reuters/USVideoEnvironment",
                "http://feeds.reuters.com/reuters/USVideoFelixSalmon",
                "http://feeds.reuters.com/reuters/USVideoGigaom",
                "http://feeds.reuters.com/reuters/USVideoLifestyle",
                "http://feeds.reuters.com/reuters/USVideoMostWatched",
                "http://feeds.reuters.com/reuters/USVideoLatest",
                "http://feeds.reuters.com/reuters/USVideoNewsmakers",
                "http://feeds.reuters.com/reuters/USVideoOddlyEnough",
                "http://feeds.reuters.com/reuters/USVideoPersonalFinance",
                "http://feeds.reuters.com/reuters/USVideoPolitics",
                "http://feeds.reuters.com/reuters/USVideoRoughCuts",
                "http://feeds.reuters.com/reuters/USVideoSmallBusiness",
                "http://feeds.reuters.com/reuters/USVideoTechnology",
                "http://feeds.reuters.com/reuters/USVideoTopNews",
                "http://feeds.reuters.com/reuters/USVideoWorldNews",
                "http://rss.cnn.com/rss/edition.rss",
                "http://rss.cnn.com/rss/edition_world.rss",
                "http://rss.cnn.com/rss/edition_africa.rss",
                "http://rss.cnn.com/rss/edition_americas.rss",
                "http://rss.cnn.com/rss/edition_asia.rss",
                "http://rss.cnn.com/rss/edition_europe.rss",
                "http://rss.cnn.com/rss/edition_meast.rss",
                "http://rss.cnn.com/rss/edition_us.rss",
                "http://rss.cnn.com/rss/money_news_international.rss",
                "http://rss.cnn.com/rss/edition_technology.rss",
                "http://rss.cnn.com/rss/edition_space.rss",
                "http://rss.cnn.com/rss/edition_entertainment.rss",
                "http://rss.cnn.com/rss/edition_sport.rss",
                "http://rss.cnn.com/rss/edition_football.rss",
                "http://rss.cnn.com/rss/edition_golf.rss",
                "http://rss.cnn.com/rss/edition_motorsport.rss",
                "http://rss.cnn.com/rss/edition_tennis.rss",
                "http://rss.cnn.com/rss/edition_travel.rss",
                "http://rss.cnn.com/rss/cnn_freevideo.rss",
                "http://rss.cnn.com/rss/cnn_latest.rss",
                "https://michalzajac.me/atom.xml",
                "http://aneta-bielska.github.io/feed.xml",
                "https://medium.com/feed/",
                "http://maque.github.io/feed.xml",
                "http://dev.nitwoe.com/feed.xml",
                "http://lukasstankiewicz.github.io/feed.xml",
                "http://dev.kielczykowski.pl/feed.xml",
                "http://klimek.link/blog/feed/",
                "http://jakubdziworski.github.io/feed.xml",
                "http://mickl.net/feed/",
                "https://jacek.migdal.pl/feed.xml",
                "http://blog.e-polecanie.pl/feed/",
                "http://dodocs.azurewebsites.net/rss",
                "http://dev.mensfeld.pl/feed/",
                "http://www.feliszewski.eu/feed/",
                "https://art-of-ai.com/feed/",
                "http://www.dobreprogramy.pl/djfoxer",
                "http://czekanski.info/feed/",
                "https://tomoitblog.wordpress.com/feed/",
                "http://codingpersona.com/feed/",
                "http://devfirststeps.blog.pl/feed/",
                "http://doriansobacki.pl/feed/",
                "http://namiekko.pl/feed/",
                "http://parkowanko.blogspot.com/feeds/posts/default",
                "http://blog.gonek.net/feed/",
                "http://kotprogramistyczny.pl/feed/",
                "https://beabest.wordpress.com/feed/",
                "http://blog.yellowmoleproductions.pl/feed/",
                "http://gronek.gq/feed/",
                "http://piatkosia.k4be.pl/wordpress/?feed=rss2",
                "http://negativeprogrammer.blogspot.com/feeds/posts/default",
                "http://devbochenek.pl/feed/",
                "https://dziewczynazpytonem.wordpress.com/feed/",
                "http://blog.chyla.org/rss",
                "http://szymekk.me/blog/feed/",
                "http://www.dedlajn.pl/feeds/posts/default",
                "http://filipcinik.azurewebsites.net/index.php/feed/",
                "http://emigd.azurewebsites.net/feed.xml",
                "http://www.devanarch.com/feed/",
                "http://knowakowski.azurewebsites.net/feed/",
                "http://www.adambac.com/feed.xml",
                "http://tomaszkorecki.com/feed/",
                "http://www.sebastiangruchacz.pl/feed/",
                "http://paweltymura.pl/feed/",
                "http://www.pyrzyk.net/feed/",
                "http://marcinszyszka.pl/feed/",
                "http://blog.kars7e.io/feed.xml",
                "http://dyzur.blogspot.com/feeds/posts/default",
                "http://mateorobiapke.blogspot.com/feeds/posts/default",
                "http://tomasz.dudziak.eu/feed/",
                "http://perceptrons.prv.pl/feed/",
                "https://dotnetcoder.wordpress.com/feed/",
                "http://jakubskoczen.pl/feed/",
                "http://krzysztofzawistowski.azurewebsites.net/?feed=rss2",
                "https://brinf.wordpress.com/feed/",
                "https://devblog.dymel.pl/feed/",
                "http://koscielniak.me/post/index.xml",
                "http://liveshare.azurewebsites.net/feed/",
                "http://blog.lantkowiak.pl/index.php/feed/",
                "http://pumiko.pl/feed.xml",
                "http://kodikable.pl/rss",
                "http://halibuti.blogspot.com/feeds/posts/default",
                "http://jaroslawstadnicki.pl/feed/",
                "https://duszekmestre.wordpress.com/feed/",
                "http://koscielski.ninja/feed/",
                "http://matma.github.io/feed.xml",
                "http://localwire.pl/feed/",
                "http://oxbow.pl/feed/",
                "http://adam.skobo.pl/?feed=rss2",
                "http://it-michal-sitko.blogspot.com/feeds/posts/default",
                "http://blog.simpleshop.pl/?feed=rss2",
                "http://kduszynski.pl/feed/",
                "https://blog.scooletz.com/feed/",
                "http://dsp.katafrakt.me/feed.xml",
                "http://www.karolpysklo.pl/?feed=rss2",
                "http://student.agh.edu.pl/~kefas/?feed=rss2",
                "http://blog.kokosa.net/syndication.axd",
                "https://slaviannblog.wordpress.com/feed/",
                "http://www.bodolsog.pl/devblog/feed/",
                "http://michalgellert.blogspot.com/feeds/posts/default",
                "http://feeds.feedburner.com/PassionateProgram",
                "http://donpiekarz.pl/feed.xml",
                "http://piotr-wandycz.pl/feed/",
                "http://adamszneider.azurewebsites.net/feed/",
                "http://www.xaocml.me/feed.xml",
                "http://sebcza.pl/feed/",
                "http://www.wearesicc.com/feed/",
                "http://tomaszjarzynski.pl/feed/",
                "http://hryniewski.net/syndication.axd",
                "http://toomanyitprojects.azurewebsites.net/feed/",
                "http://krystianbrozek.pl/feed/",
                "http://rzeczybezinternetu.blogspot.com/feeds/posts/default",
                "http://www.blog.plotnicki.net/?feed=rss2",
                "http://lukasz-jankowski.pl/feed/",
                "http://www.md-techblog.net.pl/feed/",
                "http://swistak35.com/feed.xml",
                "http://www.mikolajdemkow.pl/feed",
                "http://kubasz.esy.es/feed/",
                "http://szumiato.pl/feed/",
                "http://mateuszstanek.pl/feed/",
                "http://metodprojekt.blogspot.com/feeds/posts/default",
                "https://devprzemm.wordpress.com/feed/",
                "http://incodable.blogspot.com/feeds/posts/default",
                "http://masakradev.tk/?feed=rss2",
                "http://crynkowski.pl/feed/",
                "http://manisero.net/feed/",
                "http://programistka.net/feed/",
                "http://mbork.pl/?action=rss",
                "http://lion.net.pl/blog/feed.xml",
                "http://www.diwebsity.com/feed/",
                "http://bartoszrowinski.pl/feed/",
                "http://michalogluszka.pl/feed/",
                "http://sprobujzmiany.blogspot.com/feeds/posts/default",
                "https://adrianancymon.wordpress.com/feed/",
                "https://stitzdev.wordpress.com/feed/",
                "http://cezary.mcwronka.com.hostingasp.pl/feed/",
                "http://chmielowski.net/feed/",
                "https://damianwojcikblog.wordpress.com/feed/",
                "http://maciejskuratowski.com/feed/",
                "http://tokenbattle.blogspot.com/feeds/posts/default",
                "http://napierala.org.pl/blog/feed/",
                "http://nicholaszyl.net/feed/",
                "http://milena.mcwronka.com.hostingasp.pl/feed/",
                "http://immora.azurewebsites.net/feed/",
                "http://blog.leszczynski.it/feed/",
                "http://addictedtocreating.pl/feed/",
                "http://www.namekdev.net/feed/",
                "https://barloblog.wordpress.com/feed/",
                "http://itcraftsman.pl/feed/",
                "http://www.przemyslawowsianik.net/feed/",
                "http://www.ibpabisiak.pl/?feed=rss2",
                "http://macieklesiczka.github.io/rss",
                "http://0x00antdot.blogspot.com/feeds/posts/default",
                "http://kkustra.blogspot.com/feeds/posts/default",
                "http://msnowak.pl/feed/",
                "http://bga.pl/index.php/feed/",
                "http://blog.jhossa.net/feed/",
                "http://nowas.pl/feed/",
                "http://blog.degustudios.com/index.php/feed/",
                "http://jakubfalenczyk.com/feed/",
                "https://citygame2016.wordpress.com/feed/",
                "http://fogielpiotr.blogspot.com/feeds/posts/default",
                "http://www.straightouttacode.net/rss",
                "http://blog.creyn.pl/feed/",
                "http://ppkozlowski.pl/blog/feed/",
                "https://pablitoblogblog.wordpress.com/feed/",
                "http://csharks.blogspot.com/feeds/posts/default",
                "https://onehundredoneblog.wordpress.com/feed/",
                "http://mcupial.pl/feed/",
                "http://www.select-iot.pl/feed/",
                "http://twitop.azurewebsites.net/index.php/feed/",
                "http://brozanski.net/index.php/feed/",
                "http://arekbal.blogspot.com/feeds/posts/default",
                "http://wezewkodzie.blogspot.com/feeds/posts/default",
                "http://codinghabit.pl/feed/",
                "http://martanocon.com/?feed=rss2",
                "http://pawelrzepinski.azurewebsites.net/feed/",
                "http://paweljurczynski.pl/feed/",
                "http://pgrzesiek.pl/feed/",
                "http://krzyskowk.postach.io/feed.xml",
                "http://marcindrobik.pl/Home/rss",
                "http://michal.muskala.eu/feed.xml",
                "http://damiankedzior.com/feed/",
                "https://jporwol.wordpress.com/feed/",
                "http://takiarek.com/feed/",
                "https://ourtownapp.wordpress.com/feed/",
                "http://ggajos.com/rss",
                "http://commitandrun.pl/feed.xml",
                "http://czesio-w-it.2ap.pl/feed/",
                "http://kreskadev.azurewebsites.net/rss",
                "https://werpuc.wordpress.com/feed/",
                "http://jsdn.pl/feed/",
                "http://www.marcinwojdak.pl/?feed=rss2",
                "http://www.malachowicz.org/?feed=rss2",
                "http://slavgamebrew.com/feed/",
                "http://blog.exmoor.pl/feed",
                "https://koniecznuda.wordpress.com/feed/",
                "https://krzysztofmorcinek.wordpress.com/feed/",
                "http://blog.roobina.pl/?rss=516047c1-683c-4521-8ffd-143a0a546c85",
                "http://www.arturnet.pl/feed/",
                "http://blog.waldemarbira.pl/feed/",
                "http://justmypassions.pl/?feed=rss2",
                "http://blog.kurpio.com/feed/",
                "http://cad.bane.pl/feed/",
                "http://codestorm.pl/feed/",
                "http://charyzmatyczny-programista.blogspot.com/feeds/posts/default",
                "https://odzeradokoderablog.wordpress.com/feed/",
                "http://lukasz-zborek.pl/feed/",
                "https://ismenax.wordpress.com/feed/",
                "http://epascales.blogspot.com/feeds/posts/default",
                "http://mariuszbartosik.com/feed/",
                "http://dragonet-therrax.blog.pl/feed/",
                "http://www.sgierlowski.pl/posts/rss",
                "http://moje-zagwostki.blogspot.com/feeds/posts/default",
                "http://aragornziel.blogspot.com/feeds/posts/default",
                "http://bartmalanczuk.github.io/feed.xml",
                "http://www.winiar.pl/blog/feed/",
                "http://msaldak.pl/feed/",
                "http://krzysztofabramowicz.com/feed/",
                "https://zerojedynka.wordpress.com/feed/",
                "https://kamilhawdziejuk.wordpress.com/feed/",
                "http://paweldobrzanski.pl/feed",
                "http://polak.azurewebsites.net/rss",
                "https://bizon7nt.github.io/feed.xml",
                "http://foreverframe.pl/feed/",
                "https://bzaremba.wordpress.com/feed/",
                "http://marcinkowalczyk.pl/blog/feed/",
                "http://www.webatelier.io/blog.xml",
                "http://rutkowski.in/feed/",
                "http://jagielski.net/feed/",
                "http://plotzwi.com/feed/",
                "https://netgwg.wordpress.com/feed/",
                "http://coder-log.blogspot.com/feeds/posts/default",
                "http://dev30.pl/feed/",
                "https://chrisseroka.wordpress.com/feed/",
                "http://www.andrzejdubaj.com/feed/",
                "http://agatamroz.com.pl/feed/",
                "https://fadwick.wordpress.com/feed/",
                "http://t-code.pl/atom.xml",
                "http://zelazowy.github.io/feed.xml",
                "http://www.owsiak.org/?feed=rss2",
                "http://programuje.net/feed/",
                "http://tomaszsokol.pl/feed/",
                "http://newstech.pl/feed/",
                "http://findfriendsswift.blogspot.com/feeds/posts/default",
                "http://lazarusdev.pl/feed/",
                "http://novakov.github.io/feed.xml",
                "http://tsovek.blogspot.com/feeds/posts/default",
                "http://blog.buczaj.com/feed/",
                "http://piotrgankiewicz.com/feed/",
                "https://admincenterblog.wordpress.com/feed/",
                "https://gettoknowthebob.wordpress.com/feed/",
                "http://zszywacz.azurewebsites.net/feed/",
                "http://improsoft.blogspot.com/feeds/posts/default",
                "http://lazybitch.com/feed",
                "http://pewudev.pl/feed/",
                "http://mborowy.com/feed/",
                "http://cleancodestruggle.blogspot.com/feeds/posts/default",
                "http://lukaszmocarski.com/feed/",
                "http://blog.rakaz.pl/feed/",
                "http://ionicdsp.eu/?feed=rss2",
                "http://www.codiferent.pl/feed/",
                "http://spine.angrybits.pl/?feed=rss2",
                "http://bnowakowski.pl/en/feed/",
                "http://kazaarblog.blogspot.com/feeds/posts/default",
                "http://blog.yotkaz.me/feeds/posts/default",
                "http://langusblog.pl/index.php/feed/",
                "http://podziemiazamkul.blogspot.com/feeds/posts/default",
                "http://www.wlangiewicz.com/feed/",
                "http://resumees.net/devblog/feed/",
                "https://uwagababaprogramuje.wordpress.com/feed/",
                "http://terianil.blogspot.com/feeds/posts/default",
                "http://sirdigital.pl/index.php/feed/",
                "http://mmalczewski.pl/index.php/feed/",
                "http://cojakodze.pl/feed/",
                "http://mieczyk.vilya.pl/feed/",
                "https://jendaapkatygodniowo.wordpress.com/feed/",
                "http://dziury-w-calym.pl/feed/",
                "http://sweetprogramming.com/feed/",
                "https://branegblog.wordpress.com/feed/",
                "http://przemek.ciacka.com/feed.xml",
                "http://maciektalaska.github.io/atom.xml",
                "http://www.mguzdek.pl/feed/",
                "http://dspprojekt.blogspot.com/feeds/posts/default",
                "https://tomaszprasolek.wordpress.com/feed/",
                "http://mrojecki.azurewebsites.net/rss",
                "http://itka4yk.blogspot.com/feeds/posts/default",
                "http://whitebear.com.pl/feed/",
                "http://mzieba.com/feed/",
                "https://alpac4blog.wordpress.com/feed/",
                "http://mplonski.prv.pl/feed/",
                "http://blog.forigi.com/feed/",
                "http://www.code-addict.pl/feed",
                "http://tomaszkacmajor.pl/index.php/feed/",
                "http://marcinkruszynski.blogspot.com/feeds/posts/default",
                "http://blog.stanaszek.pl/feed/",
                "http://memforis.info/feed/"
            };

            var rssChannels = x.Select(b => new RssChannel(b, b)).ToList();
            this.database.RssChannels.AddRange(rssChannels);
            this.database.SaveChanges();
        }

        public List<RssChannel> LoadAllChannels()
        {
            return this.database.RssChannels.ToList();
        }
    }
}