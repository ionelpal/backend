﻿using Microsoft.Azure.Mobile.Server;

namespace nccloudService.DataObjects
{
    public class TodoItem : EntityData
    {
        public string UserId { get; set; }  
        public string Text { get; set; }

        public bool Complete { get; set; }
    }
}