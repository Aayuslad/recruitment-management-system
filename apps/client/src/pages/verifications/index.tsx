import { CandidatesTable } from '@/components/verifications/candidates-table';

export function Index() {
    return (
        <div className=" h-full">
            <div className="bg-muted dark:bg-slate-800/40 h-[100px] w-full flex items-center">
                <div className="px-10 flex-1">
                    <h1 className="text-2xl font-bold">Verifications</h1>
                    <p>
                        Complete pending verifications, background checks, and
                        offers to finalize candidate hiring.
                    </p>
                </div>
                <div className="w-[230px] mb-4">
                    {/* <CreateCandidateSheet /> */}
                </div>
            </div>

            <div className="w-full flex justify-evenly pt-8 pr-10">
                <div className="w-full flex flex-col items-center gap-8">
                    {/* <JobApplicationIndexTabBar /> */}
                    <div className="">
                        <CandidatesTable />
                    </div>
                </div>
            </div>
        </div>
    );
}
