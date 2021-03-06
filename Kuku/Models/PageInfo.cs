﻿using System;

namespace Kuku.Models
{
    public class PageInfo
    {
        public PageInfo(int totalItems, int? page, string url = "", int pageSize = 12)
        {
            // calculate total, start and end pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            var currentPage = page != null ? (int)page : 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 12)
                {
                    startPage = endPage - 11;
                }
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
            if (url == "")
            {
                urlPage = "?page=";
                urlFirst = "/";
            }
            else
            {
                urlPage = url + "&page=";
                urlFirst = url;
            }
        }

        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public string urlPage { get; private set; }
        public string urlFirst { get; private set; }
    }
}

