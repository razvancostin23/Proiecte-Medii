﻿using SQLite;

namespace RestaurantApp.Models
{
    public class Table
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public int Seats { get; set; }
        public string? Status { get; set; }
    }
}
