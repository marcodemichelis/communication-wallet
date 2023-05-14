using WebApi.Entities;
using WebApi.Repositories.HistoryItem;
using WebApi.Services.SignalRHubs;

namespace WebApi.Services;

public class BroadcastNewsHostedService : IHostedService
{
    public static bool PushItemsEnabled = false;
    private readonly IAsyncActionItemRepository _asyncActionItemRepository;
    private readonly ICommunicationHubServerNotifier _notifier;

    readonly List<NewsItem> _newsItems = new()
    {
        new(NewsTypes.Article, "Fatturazione delle cessioni intracomunitarie", "... Nel rinviare all’apposita voce sulla fatturazione delle cessioni intracomunitarie, si ricorda che, nelle cessioni intracomunitarie di beni, il cedente italiano deve: ...", "https://onefiscale.wolterskluwer.it/document/10DT0000033164ART4?searchId=830448789&pathId=536293fe8bc9c&offset=0&contentModuleContext=all"),
        new(NewsTypes.Marketing, "Genya CFO: Il software per prevenire la crisi di impresa e valutare correttamente lo stato di salute aziendale.", "", "https://www.wolterskluwer.com/it-it/solutions/genya-cfo"),
        new(NewsTypes.Article, "Regolarizzazione delle fatture", "... Le irregolarità nell'emissione della fattura di acquisto possono consistere anche nell' erronea qualificazione dell'operazione e del relativo regime IVA applicabile. ...", "https://onefiscale.wolterskluwer.it/document/10DT0000032753ART37?searchId=830448789&pathId=536293fe8bc9c&offset=4&contentModuleContext=all"),
        new(NewsTypes.Marketing, "Arca EVOLUTION, il software gestionale ideale per il controllo finanziario e di gestione", "", "https://statics.teams.cdn.office.net/evergreen-assets/safelinks/1/atp-safelinks.html"),
        new(NewsTypes.Article, "Autofattura", "... regolarizzazione dell’omessa o irregolare fatturazione. ...", "https://onefiscale.wolterskluwer.it/document/10DT0000032034SOMM?searchId=830448789&pathId=536293fe8bc9c&offset=12&contentModuleContext=all"),
        new(NewsTypes.Article, "Redazione del bilancio delle micro-imprese", "... Il bilancio predisposto dalla micro-imprese presenta una struttura semplificata rispetto al bilancio redatto in forma ordinaria. ...", "https://onefiscale.wolterskluwer.it/document/10DT0000032331SOMM?searchId=830454317&pathId=7b500b34fdb7e&offset=0&contentModuleContext=all"),
        new(NewsTypes.Marketing, "One FISCALE: Dall'esperienza IPSOA e il fisco la soluzione di informazione e di aggiornamento che ragiona come te e anticipa le tue esigenze.", "", "https://www.wolterskluwer.com/it-it/solutions/one/onefiscale"),
    };

    public BroadcastNewsHostedService(IAsyncActionItemRepository asyncActionItemRepository, ICommunicationHubServerNotifier notifier)
    {
        _asyncActionItemRepository = asyncActionItemRepository;
        _notifier = notifier;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var item in _newsItems)
                {
                    if (PushItemsEnabled)
                    {
                        var newItem = new AsyncActionItem(
                                            "",
                                            "",
                                            Guid.NewGuid().ToString(),
                                            item.ItemType switch { NewsTypes.Article => ItemTypes.Article, NewsTypes.Marketing => ItemTypes.Advertising },
                                            BuildHtmlMessage(item),
                                            ActionStatuses.Processed,
                                            DateTime.Now);
                        await _asyncActionItemRepository.Upsert(newItem);
                        await _notifier.NotifyNewChangesForAll();
                        await Task.Delay(60000);
                    }
                    else
                        await Task.Delay(5000);
                }
            }
        }, cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private string BuildHtmlMessage(NewsItem item)
        => item.ItemType switch
        {
            NewsTypes.Marketing => $"""MARKETING: {item.Title} <a target='_blank' href='{item.Link}'><img height='18px' src='/assets/images/megaphone.png'></a>""",
            NewsTypes.Article => $"""ARTICOLO: {item.Title} <a target='_blank' href='{item.Link}'><img height='18px' src='/assets/images/link.png'></a>""",
            _ => "tbd",
        };
}

public record NewsItem(NewsTypes ItemType, string Title, string Body, string Link);
public enum NewsTypes
{
    Article = 0,
    Marketing = 1
}