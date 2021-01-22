using System;

namespace Fortnite.Net.Model.Fortnite
{
    
    public class FortCatalogResponse
    {
        public int RefreshIntervalHrs { get; set; }
        public int DailyPurchaseHrs { get; set; }
        public DateTime Expiration { get; set; }
        public StoreFront[] StoreFronts { get; set; }
    }

    public class StoreFront
    {
        public string Name { get; set; }
        public CatalogEntry[] CatalogEntries { get; set; }
    }

    public class CatalogEntry
    {
        public string OfferId { get; set; }
        public string DevName { get; set; }
        public string OfferType { get; set; }
        public CatalogEntryPrice[] Prices { get; set; }
        public string[] Categories { get; set; }
        public int DailyLimit { get; set; }
        public int WeeklyLimit { get; set; }
        public int MonthlyLimit { get; set; }
        public bool Refundable { get; set; }
        public string[] AppStoreId { get; set; }
        public CatalogEntryRequirement[] Requirements { get; set; }
        public CatalogEntryMeta[] MetaInfo { get; set; }
        public string CatalogGroup { get; set; }
        public int CatalogGroupPriority { get; set; }
        public int SortPriority { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string DisplayAssetPath { get; set; }
        public FortItemStack[] ItemGrants { get; set; }
    }

    public class CatalogEntryRequirement
    {
        public string RequirementType { get; set; }
        public string RequiredId { get; set; }
        public int MinQuantity { get; set; }
    }

    public class CatalogEntryPrice
    {
        public string CurrencyType { get; set; }
        public string CurrencySubType { get; set; }
        public int RegularPrice { get; set; }
        public int DynamicRegularPrice { get; set; }
        public int FinalPrice { get; set; }
        public DateTime SaleExpiration { get; set; }
        public int BasePrice { get; set; }
    }
    
    public class CatalogEntryMeta 
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

}