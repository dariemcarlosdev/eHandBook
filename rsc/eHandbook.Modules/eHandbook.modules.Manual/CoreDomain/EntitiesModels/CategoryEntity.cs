﻿using eHandbook.Core.Domain;
using eHandbook.Core.Domain.Abstractions;

namespace eHandbook.modules.ManualManagement.CoreDomain.EntitiesModels
{
    public class CategoryEntity : BaseEntity, IAuditableEntity
    {

        string CategoryName { get; set; }
        string CategoryDescription { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsUpdated { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }

    }
}