﻿using WhatsApp.Models;

namespace WhatsApp.Extensions
{
    public static class QuerableExtension
    {
       
            public static IQueryable<Message> MarkUnreadAsRead(this IQueryable<Message> query, string currentUsername)
            {
                var unreadMessages = query.Where(m => m.DateRead == null
                    && m.RecipientUsername == currentUsername);

                if (unreadMessages.Any())
                {
                    foreach (var message in unreadMessages)
                    {
                        message.DateRead = DateTime.UtcNow;
                    }
                }

                return query;
            }

        }
    }

