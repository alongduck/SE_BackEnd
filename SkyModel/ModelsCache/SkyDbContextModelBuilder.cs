﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#pragma warning disable 219, 612, 618
#nullable disable

namespace MyModels
{
    public partial class SkyDbContextModel
    {
        private SkyDbContextModel()
            : base(skipDetectChanges: false, modelId: new Guid("8296eaac-abe2-4d96-95d7-f20f9449790f"), entityTypeCount: 6)
        {
        }

        partial void Initialize()
        {
            var category = CategoryEntityType.Create(this);
            var minIO = MinIOEntityType.Create(this);
            var newsArticle = NewsArticleEntityType.Create(this);
            var product = ProductEntityType.Create(this);
            var productDetail = ProductDetailEntityType.Create(this);
            var user = UserEntityType.Create(this);

            MinIOEntityType.CreateForeignKey1(minIO, productDetail);
            NewsArticleEntityType.CreateForeignKey1(newsArticle, minIO);
            ProductEntityType.CreateForeignKey1(product, category);
            ProductEntityType.CreateForeignKey2(product, productDetail);
            ProductEntityType.CreateForeignKey3(product, minIO);
            ProductEntityType.CreateForeignKey4(product, user);

            CategoryEntityType.CreateAnnotations(category);
            MinIOEntityType.CreateAnnotations(minIO);
            NewsArticleEntityType.CreateAnnotations(newsArticle);
            ProductEntityType.CreateAnnotations(product);
            ProductDetailEntityType.CreateAnnotations(productDetail);
            UserEntityType.CreateAnnotations(user);

            AddAnnotation("ProductVersion", "9.0.0");
            AddAnnotation("Proxies:ChangeTracking", false);
            AddAnnotation("Proxies:CheckEquality", false);
            AddAnnotation("Proxies:LazyLoading", true);
            AddAnnotation("Relational:MaxIdentifierLength", 128);
            AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }
    }
}
