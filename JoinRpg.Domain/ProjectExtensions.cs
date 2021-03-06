﻿using System;
using JoinRpg.DataModel;

namespace JoinRpg.Domain
{
  public static class ProjectExtensions
  {
    public static void MarkTreeModified(this Project project)
    {
      project.CharacterTreeModifiedAt = DateTime.UtcNow;
    }

    public static bool IsPlotPublished(this Project project)
    {
      return (project.Details?.PublishPlot ?? false);
    }
  }
}
