﻿using System;

namespace VKM.Admin.Models.Database.Domain
{
    public class HistoryItem
    {
        public DateTime Date { get; set; }
        public int Value { get; set; }
        public string AlgorithmName { get; set; }
    }
}