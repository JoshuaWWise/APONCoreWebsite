﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;

namespace APONCoreWebsite.Pages.Handlers
{
    public class SaveHandlerModel : ViewModelBase
    {
        [BindProperty]
        public int ID { get; set; }
        public SaveHandlerModel(IAuthService authService, IMetaTagService imts, IDataService ds) : base(authService, imts, ds)
        {

        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostSplashNewsAsync()
        {
            SplashNews SN = new SplashNews();
            SN.EndDate = DateTime.Parse(Request.Form["EndDate"]);
            SN.StartDate = DateTime.Parse(Request.Form["StartDate"]);
            SN.SplashNewsID = int.Parse(Request.Form["currentSplashID"]);
            SN.LinkID = int.Parse(Request.Form["currentSplashLinkID"]);
            SN.PostURL = Request.Form["currentSplashPostURL"];
            SN.Headline = Request.Form["headline"];
            SN.ImageURL = Request.Form["imgURL"];


            HttpResponseMessage result = await DS.PutAsync(SN, "SplashNews/UpdateSplashnews");

            return new JsonResult(result.StatusCode.ToString());


        }

        public async Task<IActionResult> OnPostDelSplashNewsAsync()
        {
            if (ID == 0)
            {
                return new JsonResult(null);
            }

            HttpResponseMessage result = await DS.DeleteAsync("SplashNews/DeleteSplashNews/" + ID);

            return new JsonResult(result.StatusCode.ToString());
        }


        public async Task<IActionResult> OnDeleteTweetAsync()
        {

            //Not implemented yet on server
            HttpResponseMessage Result = await DS.DeleteAsync("Twitter/DeleteTweet/" + ID);
            return new JsonResult(Result.StatusCode);
        }

        public async Task<IActionResult> OnPostUpdateTweetAsync()
        {
            Tweet tweet = new Tweet();
            tweet.TweetID = int.Parse(Request.Form["TweetID"]);
            tweet.TweetDate = DateTime.Parse(Request.Form["TweetDate"]);
            tweet.Text = Request.Form["TweetText"];
            tweet.MediaURL = Request.Form["MediaURL"];
            tweet.SeriesID = int.Parse(Request.Form["SeriesID"]);
         
            tweet.TwitterKeyID = int.Parse(Request.Form["TwitterKeyID"]);
            tweet.Sent = bool.Parse(Request.Form["currentCheckboxValue"]);

            HttpResponseMessage Result = await DS.PutAsync(tweet, "Twitter/UpdateTweet");
            return new JsonResult(Result.StatusCode.ToString());
        }

        public async Task<IActionResult> OnPostResetPassword()
        {
            ResetPW resetPW = new ResetPW();
            resetPW.ResetLink = Request.Form["LinkInfo"];
            resetPW.Password = Request.Form["firstPassword"];

            HttpResponseMessage Result = await DS.PostAsync(resetPW, "user/ResetPassword");

            return new JsonResult(Result.StatusCode.ToString());
        }

        public async Task<IActionResult> OnPostAddForumComment()
        {
            ForumPost FP = new ForumPost();
            FP.ForumID = int.Parse(Request.Form["ForumID"]);
            FP.Text = Request.Form["Text"];
            FP.Blocked = false;
            FP.Flagged = false;
            FP.ReplyToCommentID = -1;
            FP.ForumIndex = int.Parse(Request.Form["ForumIndex"]);
            HttpResponseMessage Result = await DS.PostAsync(FP, "forum/AddForumPost");
            string resultText = Result.StatusCode.ToString();
            if (resultText == "BadRequest")
            {
                resultText = "It looks like your sign-in session has expired. Please sign in again to comment.";
            }
            return new JsonResult(resultText);
        }


    }
}