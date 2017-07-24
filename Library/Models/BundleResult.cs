
using System;

public class BundleResult
{
    public DateTime ResultGenerated { get; set; }
    public int ResultProcessingTime { get; set; }
    public Datum[] Data { get; set; }
    public int ResultSize { get; set; }
    public int TotalSize { get; set; }
}

public class Datum
{
    public string Urn { get; set; }
    public int Version { get; set; }
    public object CreatedBy { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime LastModified { get; set; }
    public object ModifiedBy { get; set; }
    public Presentationseries PresentationSeries { get; set; }
    public bool DeletionPending { get; set; }
    public bool ApprovedByEditor { get; set; }
    public bool Dirty { get; set; }
    public DateTime _LastPrimaryBroadcast { get; set; }
    public DateTime _LastPrimaryBroadcastTime { get; set; }
    public string _LastPrimaryBroadcastUrn { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartPublish { get; set; }
    public DateTime EndPublish { get; set; }
    public string Slug { get; set; }
    public Relation[] Relations { get; set; }
    public string BundleType { get; set; }
    public string SeriesIdentifier { get; set; }
    public object[] MasterEpgSeriesIdentifiers { get; set; }
    public bool ShowWebPage { get; set; }
    public string ChannelType { get; set; }
    public bool DrChannel { get; set; }
    public string OnlineGenreText { get; set; }
    public string PrimaryChannel { get; set; }
    public string[] TitleKeywords { get; set; }
    public string _storageSize { get; set; }
    public string _storageDuration { get; set; }
    public float _storageSizeRaw { get; set; }
    public float _storageDurationRaw { get; set; }
    public DateTime _storageLastRun { get; set; }
    public bool ShowArticles { get; set; }
    public int DefaultExpiryDays { get; set; }
    public int DefaultGeoFilter { get; set; }
    public int DefaultEncrypted { get; set; }
    public string PodcastFeedPath { get; set; }
    public string SiteUrl { get; set; }
    public string BroadcastDescription { get; set; }
    public string ContactInformation { get; set; }
    public object[] SelectedContentDeliveryNetworks { get; set; }
    public string DefaultSorting { get; set; }
    public bool IsPodcastable { get; set; }
    public string Subtitle { get; set; }
    public bool _hasPrimaryAsset { get; set; }
    public object[] Assets { get; set; }
    public DateTime _LastPrimaryBroadcastWithPublicAssetTime { get; set; }
    public string _LastPrimaryBroadcastWithPublicAssetUrn { get; set; }
    public string _PublicAssetTargetTypes { get; set; }
    public DateTime _LastSortDateTimeWithPublicAsset { get; set; }
    public DateTime _LastSortDateTimeWithPublicAssetTime { get; set; }
    public string _LastSortDateTimeWithPublicAssetUrn { get; set; }
    public bool HasSeasonWithSeasonNumber { get; set; }
    public bool IsLargeBundle { get; set; }
}

public class Presentationseries
{
    public long Id { get; set; }
    public DateTime Timestamp { get; set; }
    public long Touched { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public string Type { get; set; }
    public long ParentId { get; set; }
    public int SeasonNumber { get; set; }
    public bool EpisodeNumberingValid { get; set; }
}

public class Relation
{
    public string Kind { get; set; }
    public string Urn { get; set; }
    public string Slug { get; set; }
    public string BundleType { get; set; }
}

