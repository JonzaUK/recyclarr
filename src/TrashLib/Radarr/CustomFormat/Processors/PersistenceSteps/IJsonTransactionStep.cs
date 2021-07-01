using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TrashLib.Radarr.Config;
using TrashLib.Radarr.CustomFormat.Models;
using TrashLib.Radarr.CustomFormat.Models.Cache;

namespace TrashLib.Radarr.CustomFormat.Processors.PersistenceSteps
{
    public interface IJsonTransactionStep
    {
        CustomFormatTransactionData Transactions { get; }

        void Process(
            IEnumerable<ProcessedCustomFormatData> guideCfs,
            IReadOnlyCollection<JObject> radarrCfs,
            RadarrConfig config);

        void RecordDeletions(IEnumerable<TrashIdMapping> deletedCfsInCache, List<JObject> radarrCfs);
    }
}
