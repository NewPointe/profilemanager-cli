using System.Threading.Tasks;

namespace org.newpointe.profilemanager.cli
{

    abstract class Command
    {
        abstract public Task Execute(string[] args, bool isRoot); 

    }

}