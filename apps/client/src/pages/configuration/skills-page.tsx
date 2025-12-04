import { CreateSkillDialog } from '@/components/configurations/skill/creare-skill-dialog';
import { DeleteSkillDialog } from '@/components/configurations/skill/delete-skill-dialog';
import { EditSkillDialog } from '@/components/configurations/skill/edit-skill-dialog';
import { SkillsTable } from '@/components/configurations/skill/skills-table';

export function SkillsPage() {
    return (
        <div className="h-full">
            <div className="bg-muted dark:bg-slate-800/40 h-[100px] w-full flex items-center">
                <div className="px-10 flex-1">
                    <h1 className="text-2xl font-bold">Skills</h1>
                    <p>
                        Manage all platform skills used in jobs, positions, and
                        designations.
                    </p>
                </div>
                <div className="w-[200px] mb-4">
                    <CreateSkillDialog />
                </div>
            </div>

            <div className="w-full flex justify-center pt-10">
                <div className="mr-12">
                    <SkillsTable />
                </div>
                <EditSkillDialog />
                <DeleteSkillDialog />
            </div>
        </div>
    );
}
