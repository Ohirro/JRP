﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using JoinRpg.DataModel;
using JoinRpg.Domain;

namespace JoinRpg.Web.Models
{

  public class ClaimProblemListItemViewModel : ClaimListItemViewModel
  {
    [Display(Name = "Проблема")]
    public ICollection<ProblemViewModel> Problems { get; set; }

    public static ClaimProblemListItemViewModel FromClaimProblem(IEnumerable<ClaimProblem> problem, int currentUserId, Claim claim)
    {
      var self = new ClaimProblemListItemViewModel();
      self.Assign(claim, currentUserId);
      self.Problems =
        problem.Select(p => new ProblemViewModel(p)).ToList();
      return self;
    }
  }

  public class ClaimShortListItemViewModel
  {
    [Display(Name = "Заявка")]
    public string Name { get; }

    [Display(Name = "Игрок")]
    public User Player { get; }

    public int ClaimId { get; }
    public int ProjectId { get;}

    public ClaimShortListItemViewModel (Claim claim)
    {
      ClaimId = claim.ClaimId;
      Name = claim.Name;
      Player = claim.Player;
      ProjectId = claim.ProjectId;
    }
  }

  public class ClaimListItemViewModel
  {
    [Display(Name="Заявка")]
    public string Name { get; set; }

    [Display(Name = "Игрок")]
    public User Player { get; set; }

    [Display (Name="Игра")]
    public string ProjectName { get; set; }

    [Display (Name="Статус")]
    public Claim.Status ClaimStatus { get; set; }

    [Display (Name ="Обновлена"),UIHint("EventTime")]
    public DateTime? UpdateDate { get; set; }

    [Display (Name = "Ответственный")]
    public User Responsible { get; set; }

    public User LastModifiedBy { get; set; }

    public int ProjectId { get; set; }

    public int ClaimId{ get; set; }

    public int UnreadCommentsCount { get; set; }

    public static ClaimListItemViewModel FromClaim(Claim claim, int currentUserId)
    {
      var viewModel = new ClaimListItemViewModel();
      viewModel.Assign(claim, currentUserId);
      return viewModel;
    }

    protected void Assign(Claim claim, int currentUserId)
    {
      var lastComment = claim.Comments.Where(c => c.IsVisibleToPlayer).OrderByDescending(c => c.CommentId).FirstOrDefault();

      ClaimId = claim.ClaimId;
      ClaimStatus = claim.ClaimStatus;
      Name = claim.Name;
      Player = claim.Player;
      ProjectId = claim.ProjectId;
      ProjectName = claim.Project.ProjectName;
      UpdateDate = lastComment?.LastEditTime ?? claim.CreateDate;
      Responsible = claim.ResponsibleMasterUser;
      
      LastModifiedBy = lastComment?.Author ?? claim.Player;
      UnreadCommentsCount =
        claim.Comments.Count(comment => (comment.IsVisibleToPlayer || claim.HasMasterAccess(currentUserId))
                                        && !comment.IsReadByUser(currentUserId));
    }
  }
}
