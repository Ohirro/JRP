﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JoinRpg.DataModel;

namespace JoinRpg.Web.Models.Plot
{
  public class AddPlotElementViewModel
  {
    public int ProjectId { get; set; }
    public int PlotFolderId{ get; set; }
    [Display(Name = "Текст вводной")]
    public MarkdownString Content { get; set; }
    [Display(Name = "TODO (что доделать для мастеров)"), DataType(DataType.MultilineText)]
    public string TodoField { get; set; }
    public CharacterGroupListViewModel Data { get; set; }
    public string PlotFolderName { get; set; }
  }
}