import { useGetCandidate } from '@/api/candidate-api';
import { CandidateDocumentsTable } from '@/components/candidates/candidate-documets-table';
import { DeleteCandidateDialog } from '@/components/candidates/delete-candidate-dialog';
import { EditCandidateSheet } from '@/components/candidates/edit-candidate-sheet';
import { Badge } from '@/components/ui/badge';
import { SIDEBAR_WIDTH } from '@/components/ui/sidebar';
import { Spinner } from '@/components/ui/spinner';
import { useAppStore } from '@/store';
import { ExternalLink } from 'lucide-react';
import { useParams } from 'react-router';
import { useShallow } from 'zustand/react/shallow';

export const CandidateDetailsPage = () => {
    const { id } = useParams<{ id: string }>();
    const { sidebarState } = useAppStore(
        useShallow((s) => ({
            sidebarState: s.sidebarState,
        }))
    );

    const { data, isLoading, isError } = useGetCandidate(id as string);

    if (!data || isLoading)
        return (
            <div className="w-full h-[80vh] flex items-center justify-center">
                <Spinner className="size-8" />
            </div>
        );

    if (isError)
        return (
            <div className="w-full h-[80vh] flex items-center justify-center">
                Error fetching Candidate
            </div>
        );

    return (
        <div className="h-full flex flex-col mb-10">
            <div className="h-30 flex items-center px-10 border-b ">
                <div className="space-y-1">
                    <h1 className="text-2xl font-bold">Candidate</h1>
                    <div className="space-x-3 font-semibold">
                        <span>{`${data.firstName} ${data.middleName} ${data.lastName}`}</span>
                    </div>
                </div>
                <div className="ml-auto mb-4 space-x-2">
                    <EditCandidateSheet
                        candidateId={data.id}
                        visibleTo={['Admin', 'Recruiter']}
                    />
                    <DeleteCandidateDialog
                        candidateId={data.id}
                        visibleTo={['Admin', 'Recruiter']}
                    />
                </div>
            </div>
            <div
                className="h-full flex mx-auto justify-center transition-width duration-200 ease-in-out"
                style={{
                    width: `calc(100vw - ${SIDEBAR_WIDTH} - ${sidebarState === 'opend' ? '80px' : '0px'})`,
                }}
            >
                <div className="flex-[50%] px-5 pt-8 space-y-7">
                    <div className="space-y-4">
                        <h3 className="font-semibold text-lg">Details</h3>

                        <div className="flex flex-col gap-2">
                            <div className="grid grid-cols-[150px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Name
                                </span>
                                <span className="text-sm">
                                    {`${data.firstName} ${data.middleName} ${data.lastName}`}
                                </span>
                            </div>

                            <div className="grid grid-cols-[150px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Gender
                                </span>
                                <span className="text-sm">{data.gender}</span>
                            </div>

                            <div className="grid grid-cols-[150px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Date of Birth
                                </span>
                                <span className="text-sm">
                                    {new Date(data.dob).toLocaleDateString()}
                                </span>
                            </div>

                            <div></div>
                            <div></div>

                            <div className="grid grid-cols-[150px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Email
                                </span>
                                <span className="text-sm">{data.email}</span>
                            </div>

                            <div className="grid grid-cols-[150px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Contact Number
                                </span>
                                <span className="text-sm">
                                    {data.contactNumber}
                                </span>
                            </div>

                            <div></div>
                            <div></div>

                            <div className="grid grid-cols-[150px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    College Name
                                </span>
                                <span className="text-sm">
                                    {data.collegeName}
                                </span>
                            </div>

                            <div className="grid grid-cols-[150px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Resume
                                </span>
                                <a
                                    href={data.resumeUrl}
                                    target="_blank"
                                    rel="noopener noreferrer"
                                    className="inline-flex items-center gap-1 text-sm underline hover:cursor-pointer"
                                >
                                    <span className="text-sm">View Resume</span>
                                    <ExternalLink className="w-4 h-4" />
                                </a>
                            </div>

                            <div></div>
                            <div></div>

                            <div className="grid grid-cols-[150px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Verification
                                </span>
                                <span className="text-sm">
                                    {data.isBgVerificationCompleted
                                        ? 'Verified'
                                        : 'Not Verified'}
                                </span>
                            </div>

                            {data.isBgVerificationCompleted && (
                                <div className="grid grid-cols-[150px_1fr] items-start gap-2">
                                    <span className="text-sm text-muted-foreground">
                                        Verified By
                                    </span>
                                    <span className="text-sm">
                                        {data.bgVerificationCompletedByUserName}
                                    </span>
                                </div>
                            )}
                        </div>
                    </div>

                    <div className="space-y-2">
                        <h3 className="font-semibold text-lg">Skills</h3>
                        <div className="ml-1">
                            {data?.skills.map((x) => {
                                return (
                                    <Badge
                                        variant="outline"
                                        className="text-sm font-normal pb-1.5 px-2.5 mr-1 mb-1"
                                    >
                                        {x.skillName}
                                    </Badge>
                                );
                            })}
                        </div>
                    </div>
                </div>

                <div className="flex-[50%] px-5 pt-8">
                    <CandidateDocumentsTable candidateId={data.id} />
                </div>
            </div>
        </div>
    );
};
