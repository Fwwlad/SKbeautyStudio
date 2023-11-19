﻿using Microsoft.Build.Framework;
using System;

namespace SKbeautyStudio.Db
{
    public class Appointments
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public int ServiceId { get; set; }
        public DateTime DateTime { get; set; }
        public int StatusId { get; set; }
        public int Price { get; set; }
        public Client Client { get; set; }
        public Employee Employee { get; set; }
        public Services Service { get; set; }
        public StatusesOfAppointments Status { get; set; }

    }
}