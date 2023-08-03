﻿using AdvertisementApp.Dtos.Interfaces;
using System;

namespace AdvertisementApp.Dtos
{
    public class AdvertisementAppUserCreateDto:IDto
    {
        public int AdvertisementId { get; set; }

        public int AppUserId { get; set; }

        public int AdvertisementAppUserStatusId { get; set; }

        public int MilitaryStatusId { get; set; }  //lookup table

        public DateTime? EndDate { get; set; }
        public int WorkExperience { get; set; }

        public string CvPath { get; set; }
    }
}
