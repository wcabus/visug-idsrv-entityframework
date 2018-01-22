using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public static class DbContextExtensions
    {
        public static void SetupKey<T>(this EntityTypeBuilder<T> entityTypeBuilder,
            Expression<Func<T, object>> keyExpression)
            where T : class
        {
            entityTypeBuilder.HasKey(keyExpression);
            entityTypeBuilder.Property(keyExpression)
                .ValueGeneratedOnAdd();
        }

        public static PropertyBuilder<TProperty> SetupUnicodeString<T, TProperty>(this EntityTypeBuilder<T> entityTypeBuilder,
            Expression<Func<T, TProperty>> propertyExpression, int? maxLength = null)
            where T : class
        {
            var propertyBuilder = entityTypeBuilder.Property(propertyExpression);
            if (maxLength != null)
            {
                propertyBuilder = propertyBuilder.HasMaxLength(maxLength.Value);
            }

            return propertyBuilder.IsUnicode().IsRequired();
        }
    }
}