using System.Collections.Generic;
using System.Linq;
using JoinRpg.DataModel;
using JoinRpg.Web.Helpers;

namespace JoinRpg.Web.Models
{
  public class CharacterGroupListViewModel
  {
    public int ProjectId { get; set; }

    public string ProjectName { get; set; }

    public IList<CharacterGroupListItemViewModel> Groups { get; set; }

    public static CharacterGroupListViewModel FromGroup(CharacterGroup group, bool showEditControls = false)
    {
      return new CharacterGroupListViewModel()
      {
        ProjectId = group.ProjectId,
        ProjectName = group.Project.ProjectName,
        Groups = new CharacterGroupHierarchyBuilder(group).Generate(),
        ShowEditControls = showEditControls,
      };
    }

    public static CharacterGroupListViewModel FromGroupForEdit(CharacterGroup group) => FromGroup(group, true);

    public bool ShowEditControls { get; set; }

    //TODO: unit tests
    private class CharacterGroupHierarchyBuilder
    {
      private CharacterGroup Root { get; }

      private IList<int> AlreadyOutputedGroups { get; } = new List<int>();
      private IList<int> AlreadyOutputedChars { get; } = new List<int>();

      private IList<CharacterGroupListItemViewModel> Results { get; } = new List<CharacterGroupListItemViewModel>();

      public CharacterGroupHierarchyBuilder(CharacterGroup root)
      {
        Root = root;
      }

      public IList<CharacterGroupListItemViewModel> Generate()
      {
        GenerateFrom(Root, 0, new List<CharacterGroup>());
        return Results;
      }

      private void GenerateFrom(CharacterGroup characterGroup, int deepLevel, IList<CharacterGroup> pathToTop)
      {
        var vm = new CharacterGroupListItemViewModel()
        {
          CharacterGroupId = characterGroup.CharacterGroupId,
          DeepLevel = deepLevel,
          Name = characterGroup.CharacterGroupName,
          FirstCopy = !AlreadyOutputedGroups.Contains(characterGroup.CharacterGroupId),
          AvaiableDirectSlots = characterGroup.AvaiableDirectSlots,
          Characters = characterGroup.Characters.Select(GenerateCharacter).ToList(),
          Description = characterGroup.Description.ToHtmlString(),
          Path = pathToTop.Where(cg => !cg.IsRoot).Select(cg => cg.CharacterGroupName)
        };
        Results.Add(vm);

        if (!vm.FirstCopy)
          return;

        AlreadyOutputedGroups.Add(characterGroup.CharacterGroupId);

        foreach (var childGroup in characterGroup.ActiveChildGroups)
        {
          var characterGroups =  pathToTop.Union(new [] { characterGroup }).ToList();
          GenerateFrom(childGroup, deepLevel + 1, characterGroups);
        }
      }

      private CharacterViewModel GenerateCharacter(Character arg)
      {
        var vm = new CharacterViewModel()
        {
          CharacterId = arg.CharacterId,
          CharacterName = arg.CharacterName,
          IsFirstCopy = !AlreadyOutputedChars.Contains(arg.CharacterId),
          IsAvailable = arg.IsAvailable,
          Description =  arg.Description.ToHtmlString()
        };
        if (vm.IsFirstCopy)
        {
          AlreadyOutputedChars.Add(vm.CharacterId);
        }
        return vm;
      }
    }
  }
}