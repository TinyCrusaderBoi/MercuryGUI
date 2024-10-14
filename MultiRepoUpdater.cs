using LibGit2Sharp;
using System;
using System.IO;
using System.Linq;

public class MultiRepoUpdater
{
    // Get the base directory where the application is running
    private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

    // Use relative paths for each repository
    private string mercuryGuiPath;
    private string mercuryRepoPath;

    public MultiRepoUpdater()
    {
        // Assuming "MercuryGui" and "Mercury" are in subfolders of the base directory
        mercuryGuiPath = Path.Combine(baseDirectory, "MercuryGui");
        mercuryRepoPath = Path.Combine(baseDirectory, "Mercury");
    }

    public void CheckAndUpdateRepos()
    {
        Console.WriteLine("Checking for updates in MercuryGui...");
        UpdateRepo(mercuryGuiPath);

        Console.WriteLine("Checking for updates in Mercury...");
        UpdateRepo(mercuryRepoPath);
    }

    private void UpdateRepo(string repoPath)
    {
        if (!Directory.Exists(repoPath))
        {
            Console.WriteLine($"Repository folder not found: {repoPath}");
            return;
        }

        using (var repo = new Repository(repoPath))
        {
            var remote = repo.Network.Remotes["origin"];
            Commands.Fetch(repo, remote.Name, remote.FetchRefSpecs.Select(x => x.Specification), null, "");

            var localCommit = repo.Head.Tip;
            var remoteCommit = repo.Branches["refs/remotes/origin/main"].Tip;  // Assuming 'main' branch

            if (localCommit.Sha != remoteCommit.Sha)
            {
                Console.WriteLine($"Update available for {repoPath}. Pulling changes...");
                PullLatestChanges(repo);
            }
            else
            {
                Console.WriteLine($"No updates available for {repoPath}.");
            }
        }
    }

    private void PullLatestChanges(Repository repo)
    {
        var signature = new Signature(new Identity("User", "email@example.com"), DateTimeOffset.Now);
        var mergeResult = Commands.Pull(repo, signature, null);

        if (mergeResult.Status == MergeStatus.Conflicts)
        {
            Console.WriteLine("Merge conflicts detected!");
            // Handle conflicts or notify the user.
        }
        else
        {
            Console.WriteLine("Update completed successfully.");
        }
    }
}
