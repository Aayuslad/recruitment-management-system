import { CreateSkillDialog } from '@/components/features/jobs/creare-skill-dialog';
import { EditSkillDialog } from '@/components/features/jobs/edit-skill-dialog';
import { SkillsTable } from '@/components/features/jobs/skills-table';

function SkillsSectionPage() {
    return (
        <div className=" h-full">
            <div className="bg-popover dark:bg-popover h-[100px] w-full flex items-center">
                <div className="px-10 flex-1">
                    <h1 className="text-2xl font-bold">Skills</h1>
                    <p>
                        Manage all platform skills used in jobs, positions, and
                        designations.
                    </p>
                </div>
                <div className="w-[200px]">
                    <CreateSkillDialog />
                </div>
            </div>

            <div className="w-full flex justify-evenly pt-10">
                <SkillsTable />

                <div className="border w-[300px]">
                    <h2>Audit / skill details</h2>
                </div>

                <EditSkillDialog />
            </div>
        </div>
    );
}

export default SkillsSectionPage;
