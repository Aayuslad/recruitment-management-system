import { CandidatesTable } from '@/components/candidates/candidates-table';
import { CreateCandidateSheet } from '@/components/candidates/create-candidate-sheet';

export function Index() {
    return (
        <div className=" h-full">
            <div className="bg-muted dark:bg-slate-800/40 h-[100px] w-full flex items-center">
                <div className="px-10 flex-1">
                    <h1 className="text-2xl font-bold">Candidates</h1>
                    <p>
                        View and manage all candidates associated with your
                        recruitment process.
                    </p>
                </div>
                <div className="w-[230px] mb-4">
                    <CreateCandidateSheet visibleTo={['Admin', 'Recruiter']} />
                </div>
            </div>

            <div className="w-full flex justify-evenly pt-10">
                <div className="w-full flex flex-col items-center">
                    <div className="mr-12 w-fit">
                        <CandidatesTable />
                    </div>
                </div>
            </div>
        </div>
    );
}
