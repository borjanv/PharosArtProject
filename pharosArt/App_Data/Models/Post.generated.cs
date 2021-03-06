//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder v3.0.7.99
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.ModelsBuilder;
using Umbraco.ModelsBuilder.Umbraco;

namespace Umbraco.Web.PublishedContentModels
{
	/// <summary>Post</summary>
	[PublishedContentModel("post")]
	public partial class Post : Menu
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "post";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public Post(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Post, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Grid Content
		///</summary>
		[ImplementPropertyType("gridContent")]
		public Newtonsoft.Json.Linq.JToken GridContent
		{
			get { return this.GetPropertyValue<Newtonsoft.Json.Linq.JToken>("gridContent"); }
		}

		///<summary>
		/// Page Description
		///</summary>
		[ImplementPropertyType("metaDescription")]
		public string MetaDescription
		{
			get { return this.GetPropertyValue<string>("metaDescription"); }
		}

		///<summary>
		/// Page Keywords
		///</summary>
		[ImplementPropertyType("metaKeywords")]
		public string MetaKeywords
		{
			get { return this.GetPropertyValue<string>("metaKeywords"); }
		}

		///<summary>
		/// Author
		///</summary>
		[ImplementPropertyType("postAuthor")]
		public string PostAuthor
		{
			get { return this.GetPropertyValue<string>("postAuthor"); }
		}

		///<summary>
		/// postPicker
		///</summary>
		[ImplementPropertyType("postPicker")]
		public IEnumerable<IPublishedContent> PostPicker
		{
			get { return this.GetPropertyValue<IEnumerable<IPublishedContent>>("postPicker"); }
		}

		///<summary>
		/// Subtitle
		///</summary>
		[ImplementPropertyType("postSubtitle")]
		public string PostSubtitle
		{
			get { return this.GetPropertyValue<string>("postSubtitle"); }
		}

		///<summary>
		/// Title
		///</summary>
		[ImplementPropertyType("postTitle")]
		public string PostTitle
		{
			get { return this.GetPropertyValue<string>("postTitle"); }
		}

		///<summary>
		/// slideImg
		///</summary>
		[ImplementPropertyType("slideImg")]
		public IEnumerable<IPublishedContent> SlideImg
		{
			get { return this.GetPropertyValue<IEnumerable<IPublishedContent>>("slideImg"); }
		}

		///<summary>
		/// Thumbnail
		///</summary>
		[ImplementPropertyType("thumbnail")]
		public IPublishedContent Thumbnail
		{
			get { return this.GetPropertyValue<IPublishedContent>("thumbnail"); }
		}
	}
}
