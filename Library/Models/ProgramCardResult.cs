
using System;

public class ProgramCardResult
{
    public DateTime ResultGenerated { get; set; }
    public int ResultProcessingTime { get; set; }
    public Datum1[] Data { get; set; }
    public int ResultSize { get; set; }
    public int TotalSize { get; set; }
}

public class Datum1
{
    public string Urn { get; set; }
    public int Version { get; set; }
    public object CreatedBy { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime LastModified { get; set; }
    public object ModifiedBy { get; set; }
    public string ProductionNumber { get; set; }
    public string PrimaryChannel { get; set; }
    public string Site { get; set; }
    public long PrimaryOnDemandPublicationId { get; set; }
    public DateTime SortDateTime { get; set; }
    public bool LinkedToOnDemandPublication { get; set; }
    public bool Dirty { get; set; }
    public string ChannelType { get; set; }
    public string GenreCode { get; set; }
    public string GenreText { get; set; }
    public string OnlineGenreText { get; set; }
    public int ProductionYear { get; set; }
    public string ProductionCountry { get; set; }
    public Production1 Production { get; set; }
    public DateTime PrimaryBroadcastStartTime { get; set; }
    public string PrimaryBroadcastChannel { get; set; }
    public string PrimaryBroadcastWhatsOnUri { get; set; }
    public string Subtitle { get; set; }
    public Distributionstatus[] DistributionStatuses { get; set; }
    public string PrimaryAssetKind { get; set; }
    public string PrimaryAssetUri { get; set; }
    public DateTime PrimaryAssetStartPublish { get; set; }
    public DateTime PrimaryAssetEndPublish { get; set; }
    public DateTime PrimaryAssetLastModified { get; set; }
    public int PrimaryAssetDurationInMilliseconds { get; set; }
    public bool PrimaryAssetRestrictedToDenmark { get; set; }
    public string AssetTargetTypes { get; set; }
    public string RapId { get; set; }
    public Embedpath[] EmbedPaths { get; set; }
    public bool PrimaryAssetDownloadable { get; set; }
    public int SeasonNumber { get; set; }
    public int EpisodeNumber { get; set; }
    public bool PrevHadPublicPrimaryAsset { get; set; }
    public bool HasPublicPrimaryAsset { get; set; }
    public string Title { get; set; }
    public object Description { get; set; }
    public DateTime StartPublish { get; set; }
    public DateTime EndPublish { get; set; }
    public string Slug { get; set; }
    public Asset[] Assets { get; set; }
    public Relation1[] Relations { get; set; }
    public object SiteUrl { get; set; }
    public string CardType { get; set; }
    public Broadcast[] Broadcasts { get; set; }
    public Ondemandpublication[] OnDemandPublications { get; set; }
    public string PresentationUri { get; set; }
}

public class Production1
{
    public string[] __unset { get; set; }
    public DateTime Timestamp { get; set; }
    public long Touched { get; set; }
    public string Number { get; set; }
    public long PresentationSeriesId { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
    public string TitleAlternative { get; set; }
    public string PunchLineFlow { get; set; }
    public int Year { get; set; }
    public string Country { get; set; }
    public DateTime CreateTime { get; set; }
    public string GenreCode { get; set; }
    public string GenreText { get; set; }
    public int PresentationEpisodeNumber { get; set; }
}

public class Distributionstatus
{
    public DateTime Timestamp { get; set; }
    public string Status { get; set; }
    public string Url { get; set; }
}

public class Embedpath
{
    public string Path { get; set; }
    public DateTime LastSeen { get; set; }
}

public class Asset
{
    public string Kind { get; set; }
    public string Uri { get; set; }
    public DateTime StartPublish { get; set; }
    public DateTime EndPublish { get; set; }
    public string ContentType { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public int Size { get; set; }
    public bool Trashed { get; set; }
    public string AssetScreenshotSourceUrl { get; set; }
    public DateTime LastModified { get; set; }
    public int DurationInMilliseconds { get; set; }
    public bool RestrictedToDenmark { get; set; }
    public bool Downloadable { get; set; }
    public bool Encrypted { get; set; }
    public string Target { get; set; }
    public Link[] Links { get; set; }
    public object[] SubtitlesList { get; set; }
}

public class Link
{
    public string Uri { get; set; }
    public string EncryptedUri { get; set; }
    public string HardSubtitlesType { get; set; }
    public string FileFormat { get; set; }
    public string Target { get; set; }
}

public class Relation1
{
    public string Kind { get; set; }
    public string Urn { get; set; }
    public string Slug { get; set; }
    public string BundleType { get; set; }
}

public class Broadcast
{
    public DateTime AnnouncedEndTime { get; set; }
    public DateTime AnnouncedStartTime { get; set; }
    public string Channel { get; set; }
    public bool IsRerun { get; set; }
    public string WhatsOnUri { get; set; }
    public string[] __unset { get; set; }
    public DateTime PresentationEndTime { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime BroadcastDate { get; set; }
    public long FirstPartOid { get; set; }
    public long TransmissionOid { get; set; }
    public bool VideoWidescreen { get; set; }
    public string Title { get; set; }
    public string Punchline { get; set; }
    public string Description { get; set; }
    public int ProductionYear { get; set; }
    public string ProductionCountry { get; set; }
    public string _ProductionTag { get; set; }
    public string Key { get; set; }
}

public class Ondemandpublication
{
    public long Id { get; set; }
    public DateTime Timestamp { get; set; }
    public long Touched { get; set; }
    public DateTime StartPublish { get; set; }
    public DateTime EndPublish { get; set; }
    public bool Geofiltered { get; set; }
    public bool Downloadable { get; set; }
    public bool Encrypted { get; set; }
    public string Title { get; set; }
    public object Description { get; set; }
    public string[] Platforms { get; set; }
    public string BrandingChannel { get; set; }
    public DateTime CalculatedEndPublish { get; set; }
}
