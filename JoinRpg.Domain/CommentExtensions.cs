﻿using System;
using System.Collections.Generic;
using System.Linq;
using JoinRpg.DataModel;

namespace JoinRpg.Domain
{
  public static  class CommentExtensions
  {
    public static bool IsReadByUser(this Comment comment, int userId)
    {
      return comment.AuthorUserId == userId 
        || comment.Claim.Comments.Where(c => c.AuthorUserId == userId).Any(c => c.CommentId > comment.CommentId)
        || comment.Claim.GetWatermark(userId) >= comment.CommentId;
    }

    private static int GetWatermark(this Claim claim, int userId)
    {
      return claim.Watermarks.OrderByDescending(wm => wm.CommentId).FirstOrDefault(wm => wm.UserId == userId)?.CommentId  ?? 0;
    }

    public static IEnumerable<Comment> InLastXDays(this IEnumerable<Comment> masterAnswers, int days)
    {
      return masterAnswers.Where(comment => DateTime.UtcNow.Subtract(comment.CreatedTime) < TimeSpan.FromDays(days));
    }
  }
}
