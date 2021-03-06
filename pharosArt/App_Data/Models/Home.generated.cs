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
	/// <summary>Home</summary>
	[PublishedContentModel("Home")]
	public partial class Home : PublishedContentModel
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "Home";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public Home(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Home, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Banner Title
		///</summary>
		[ImplementPropertyType("bannerTitle")]
		public string BannerTitle
		{
			get { return this.GetPropertyValue<string>("bannerTitle"); }
		}

		///<summary>
		/// Featured large
		///</summary>
		[ImplementPropertyType("contentPicker")]
		public IEnumerable<IPublishedContent> ContentPicker
		{
			get { return this.GetPropertyValue<IEnumerable<IPublishedContent>>("contentPicker"); }
		}

		///<summary>
		/// Delete item page
		///</summary>
		[ImplementPropertyType("deleteItemPage")]
		public IPublishedContent DeleteItemPage
		{
			get { return this.GetPropertyValue<IPublishedContent>("deleteItemPage"); }
		}

		///<summary>
		/// Edit item page
		///</summary>
		[ImplementPropertyType("editItemPage")]
		public IPublishedContent EditItemPage
		{
			get { return this.GetPropertyValue<IPublishedContent>("editItemPage"); }
		}

		///<summary>
		/// Edit profile page
		///</summary>
		[ImplementPropertyType("editProfilePage")]
		public IPublishedContent EditProfilePage
		{
			get { return this.GetPropertyValue<IPublishedContent>("editProfilePage"); }
		}

		///<summary>
		/// Facebook url: Enter the full url for your facebook page (including the http/ https)
		///</summary>
		[ImplementPropertyType("facebookUrl")]
		public string FacebookUrl
		{
			get { return this.GetPropertyValue<string>("facebookUrl"); }
		}

		///<summary>
		/// Featured small
		///</summary>
		[ImplementPropertyType("featuredSmall")]
		public IEnumerable<IPublishedContent> FeaturedSmall
		{
			get { return this.GetPropertyValue<IEnumerable<IPublishedContent>>("featuredSmall"); }
		}

		///<summary>
		/// Banner Image
		///</summary>
		[ImplementPropertyType("image")]
		public IPublishedContent Image
		{
			get { return this.GetPropertyValue<IPublishedContent>("image"); }
		}

		///<summary>
		/// Instagram url: Enter the full url for your instagram page (including the http/ https)
		///</summary>
		[ImplementPropertyType("instagramUrl")]
		public string InstagramUrl
		{
			get { return this.GetPropertyValue<string>("instagramUrl"); }
		}

		///<summary>
		/// Members media root folder
		///</summary>
		[ImplementPropertyType("membersMediaRootFolder")]
		public IPublishedContent MembersMediaRootFolder
		{
			get { return this.GetPropertyValue<IPublishedContent>("membersMediaRootFolder"); }
		}

		///<summary>
		/// Pages Number
		///</summary>
		[ImplementPropertyType("pagesNumber")]
		public int PagesNumber
		{
			get { return this.GetPropertyValue<int>("pagesNumber"); }
		}

		///<summary>
		/// Profile Page
		///</summary>
		[ImplementPropertyType("profilePage")]
		public IPublishedContent ProfilePage
		{
			get { return this.GetPropertyValue<IPublishedContent>("profilePage"); }
		}

		///<summary>
		/// Public profile page
		///</summary>
		[ImplementPropertyType("publicProfilePage")]
		public IPublishedContent PublicProfilePage
		{
			get { return this.GetPropertyValue<IPublishedContent>("publicProfilePage"); }
		}

		///<summary>
		/// Title
		///</summary>
		[ImplementPropertyType("title")]
		public string Title
		{
			get { return this.GetPropertyValue<string>("title"); }
		}

		///<summary>
		/// Twitter url: Enter the full url for your twitter page (including the http/ https)
		///</summary>
		[ImplementPropertyType("twitterUrl")]
		public string TwitterUrl
		{
			get { return this.GetPropertyValue<string>("twitterUrl"); }
		}

		///<summary>
		/// Video thumbnails media folder
		///</summary>
		[ImplementPropertyType("videoThumbnailsMediaFolder")]
		public IPublishedContent VideoThumbnailsMediaFolder
		{
			get { return this.GetPropertyValue<IPublishedContent>("videoThumbnailsMediaFolder"); }
		}
	}
}
