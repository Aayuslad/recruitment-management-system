import { useGetCandidate } from '@/api/candidate-api';
import { useGetJobApplication } from '@/api/job-application-api';
import { useGetJobOpening } from '@/api/job-opening-api';
import { AddFeedbackDialog } from '@/components/jobApplications/internal/add-feedback-dialog';
import { MarkHiredButton } from '@/components/jobApplications/stateActionButtons/mark-hired';
import { MarkInterviwedButton } from '@/components/jobApplications/stateActionButtons/mark-intervied';
import { MarkOfferedButton } from '@/components/jobApplications/stateActionButtons/mark-offered';
import { PutOnHoldButton } from '@/components/jobApplications/stateActionButtons/put-on-hold';
import { RejectButton } from '@/components/jobApplications/stateActionButtons/reject';
import { RollbackStatusButton } from '@/components/jobApplications/stateActionButtons/rollback-status';
import { ShortlistButton } from '@/components/jobApplications/stateActionButtons/shortlist';
import { UnHoldButton } from '@/components/jobApplications/stateActionButtons/unhold';
import { Badge } from '@/components/ui/badge';
import { SIDEBAR_WIDTH } from '@/components/ui/sidebar';
import { Spinner } from '@/components/ui/spinner';
import {
    Tooltip,
    TooltipContent,
    TooltipTrigger,
} from '@/components/ui/tooltip';
import { useAppStore } from '@/store';
import { ExternalLink, MoveRight } from 'lucide-react';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router';
import { useShallow } from 'zustand/react/shallow';

type SkillType = {
    skillId: string;
    skillName: string;
    skillType?: string;
    minExperienceYears?: number | null;
};

export const JobApplicationDetailsPage = () => {
    const { id } = useParams<{ id: string }>();
    const { sidebarState } = useAppStore(
        useShallow((s) => ({
            sidebarState: s.sidebarState,
        }))
    );
    const [matchedSkills, setMatchedSkills] = useState<SkillType[]>([]);
    const [missingSkills, setMissingSkills] = useState<SkillType[]>([]);
    const [additionalSkills, setAdditionalSkills] = useState<SkillType[]>([]);

    const {
        data: jobApplication,
        isLoading: isJobApplicationLoading,
        isError: isJobApplicationError,
    } = useGetJobApplication(id as string);
    const {
        data: candidate,
        isLoading: isCandidateLoading,
        isError: isCandidateError,
    } = useGetCandidate(jobApplication?.candidateId);
    const {
        data: jobOpening,
        isLoading: isJobOpeningLoading,
        isError: isJobOpeningError,
    } = useGetJobOpening(jobApplication?.jobOpeningId);

    useEffect(() => {
        if (candidate && jobOpening) {
            const matched = jobOpening.skills.filter((skill) =>
                candidate.skills.some((s) => s.skillId === skill.skillId)
            );
            const missing = jobOpening.skills.filter(
                (skill) =>
                    !candidate.skills.some((s) => s.skillId === skill.skillId)
            );
            const additional = candidate.skills.filter(
                (skill) =>
                    !jobOpening.skills.some((s) => s.skillId === skill.skillId)
            );
            setMatchedSkills(matched);
            setMissingSkills(missing);
            setAdditionalSkills(additional);
        }
    }, [candidate, jobOpening]);

    if (
        !(jobApplication && candidate && jobOpening) ||
        isJobApplicationLoading ||
        isCandidateLoading ||
        isJobOpeningLoading
    )
        return (
            <div className="w-full h-[80vh] flex items-center justify-center">
                <Spinner className="size-8" />
            </div>
        );

    if (isCandidateError || isJobApplicationError || isJobOpeningError)
        return (
            <div className="w-full h-[80vh] flex items-center justify-center">
                Error fetching data
            </div>
        );

    return (
        <div className="h-full flex flex-col mb-10">
            <div className="h-40 flex items-center px-10 border-b">
                <div className="space-y-1">
                    <div className="flex space-x-2">
                        <h1 className="text-2xl font-bold">Job Application</h1>
                    </div>

                    <div className="flex items-center gap-3 text-sm text-muted-foreground">
                        <div>{`${candidate.firstName} ${candidate.middleName} ${candidate.lastName}`}</div>
                        <span>•</span>
                        <span>{jobApplication.designation}</span>
                        <span>•</span>
                        <span>
                            Applied on{' '}
                            {new Date(
                                jobApplication.appliedAt
                            ).toLocaleDateString()}
                        </span>
                    </div>
                </div>

                <div className="ml-auto flex items-center gap-3">
                    <span className="px-3 py-1 rounded-full text-sm font-semibold border">
                        {jobApplication.status}
                    </span>
                </div>
            </div>

            <div
                className="h-full flex mx-auto justify-center transition-width duration-200 ease-in-out"
                style={{
                    width: `calc(100vw - ${SIDEBAR_WIDTH} - ${
                        sidebarState === 'opend' ? '80px' : '0px'
                    })`,
                }}
            >
                <div className="flex-[50%] px-5 pt-8 space-y-6 ">
                    <div className="space-y-4">
                        {/* <h3 className="font-semibold text-lg">Overview</h3> */}

                        <div className="flex flex-col gap-2">
                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Candidate Name
                                </span>
                                <span className="text-sm">{`${candidate.firstName} ${candidate.middleName} ${candidate.lastName}`}</span>
                            </div>

                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Resume
                                </span>
                                <a
                                    href={candidate.resumeUrl}
                                    target="_blank"
                                    rel="noopener noreferrer"
                                    className="inline-flex items-center gap-1 text-sm underline hover:cursor-pointer"
                                >
                                    <span className="text-sm">View Resume</span>
                                    <ExternalLink className="w-4 h-4" />
                                </a>
                            </div>
                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Candidate
                                </span>
                                <a
                                    href={candidate.resumeUrl}
                                    target="_blank"
                                    rel="noopener noreferrer"
                                    className="inline-flex items-center gap-1 text-sm underline hover:cursor-pointer"
                                >
                                    <span className="text-sm">
                                        View Candidate
                                    </span>
                                    <ExternalLink className="w-4 h-4" />
                                </a>
                            </div>
                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Job Opening
                                </span>
                                <a
                                    href={candidate.resumeUrl}
                                    target="_blank"
                                    rel="noopener noreferrer"
                                    className="inline-flex items-center gap-1 text-sm underline hover:cursor-pointer"
                                >
                                    <span className="text-sm">
                                        View Job Opening
                                    </span>
                                    <ExternalLink className="w-4 h-4" />
                                </a>
                            </div>

                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Average Feedback Rating
                                </span>
                                <span className="text-sm">
                                    {jobApplication.avgRating
                                        ? `${jobApplication.avgRating} / 10`
                                        : 'N/A'}
                                </span>
                            </div>
                        </div>
                    </div>

                    <div className="space-y-4">
                        <h3 className="font-semibold text-lg">
                            Skill Evaluation
                        </h3>

                        <div className="flex flex-col gap-2">
                            <div className="space-y-1.5">
                                <h4>Matched Skills</h4>

                                <div>
                                    {[
                                        ...matchedSkills.filter(
                                            (x) => x.skillType === 'Required'
                                        ),
                                        ...matchedSkills.filter(
                                            (x) => x.skillType === 'Preferred'
                                        ),
                                    ].map((skill) => {
                                        return (
                                            <Tooltip key={skill.skillId}>
                                                <TooltipTrigger asChild>
                                                    <Badge
                                                        variant="outline"
                                                        className={`text-sm font-normal pb-1.5 px-2.5 mr-1 mb-1 ${skill.skillType === 'Required' ? 'border-red-400' : ''} ${skill.skillType === 'Preferred' ? 'border-blue-400' : 'border-slate-400'}`}
                                                    >
                                                        <span>
                                                            {skill.skillName}
                                                        </span>
                                                        {skill.minExperienceYears !==
                                                            0 && (
                                                            <span className="text-xs -mb-1 pb-[1px] px-1.5 bg-accent rounded-2xl">
                                                                {
                                                                    skill.minExperienceYears
                                                                }
                                                            </span>
                                                        )}
                                                    </Badge>
                                                </TooltipTrigger>
                                                <TooltipContent>
                                                    <p className="space-x-1.5 mb-1">
                                                        <span className="font-semibold">
                                                            Skill Requirement
                                                            Type:
                                                        </span>
                                                        <span>
                                                            {skill.skillType}
                                                        </span>
                                                    </p>
                                                    <p className="space-x-1.5">
                                                        <span className="font-semibold">
                                                            Required Minimum
                                                            Experience:
                                                        </span>
                                                        <span>
                                                            {
                                                                skill.minExperienceYears
                                                            }{' '}
                                                            year
                                                        </span>
                                                    </p>
                                                </TooltipContent>
                                            </Tooltip>
                                        );
                                    })}
                                </div>
                            </div>

                            <div className="space-y-1.5">
                                <h4>Missing Skills</h4>

                                <div>
                                    {[
                                        ...missingSkills.filter(
                                            (x) => x.skillType === 'Required'
                                        ),
                                        ...missingSkills.filter(
                                            (x) => x.skillType === 'Preferred'
                                        ),
                                    ].map((x) => {
                                        return (
                                            <Tooltip key={x.skillId}>
                                                <TooltipTrigger asChild>
                                                    <Badge
                                                        variant="outline"
                                                        className={`text-sm font-normal pb-1.5 px-2.5 mr-1 mb-1 ${x.skillType === 'Required' ? 'border-red-400' : ''} ${x.skillType === 'Preferred' ? 'border-blue-400' : 'border-slate-400'}`}
                                                    >
                                                        <span>
                                                            {x.skillName}
                                                        </span>
                                                        {x.minExperienceYears !==
                                                            0 && (
                                                            <span className="text-xs -mb-1 pb-[1px] px-1.5 bg-accent rounded-2xl">
                                                                {
                                                                    x.minExperienceYears
                                                                }
                                                            </span>
                                                        )}
                                                    </Badge>
                                                </TooltipTrigger>
                                                <TooltipContent>
                                                    <p className="space-x-1.5 mb-1">
                                                        <span className="font-semibold">
                                                            Skill Requirement
                                                            Type:
                                                        </span>
                                                        <span>
                                                            {x.skillType}
                                                        </span>
                                                    </p>
                                                    <p className="space-x-1.5">
                                                        <span className="font-semibold">
                                                            Required Minimum
                                                            Experience:
                                                        </span>
                                                        <span>
                                                            {
                                                                x.minExperienceYears
                                                            }{' '}
                                                            year
                                                        </span>
                                                    </p>
                                                </TooltipContent>
                                            </Tooltip>
                                        );
                                    })}
                                </div>
                            </div>

                            <div className="space-y-1.5">
                                <h4>Additional Skills</h4>

                                <div>
                                    {additionalSkills.map((x) => {
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
                    </div>
                </div>

                <div className="flex-[50%] px-5 pt-8 space-y-6 ">
                    <div className="space-y-1">
                        <h3 className="font-semibold text-lg">
                            Status Actions
                        </h3>

                        <div className="border rounded-xl p-3 flex gap-3">
                            {/* At 'APPLIED' state */}
                            {jobApplication.status === 'Applied' && (
                                <>
                                    <ShortlistButton
                                        jobApplicationId={jobApplication.id}
                                    />
                                    <RejectButton
                                        jobApplicationId={jobApplication.id}
                                    />
                                    <PutOnHoldButton
                                        jobApplicationId={jobApplication.id}
                                    />
                                </>
                            )}

                            {/* At 'SHORTLISTED' state */}
                            {jobApplication.status === 'Shortlisted' && (
                                <>
                                    <MarkInterviwedButton
                                        jobApplicationId={jobApplication.id}
                                    />
                                    <RejectButton
                                        jobApplicationId={jobApplication.id}
                                    />
                                    <PutOnHoldButton
                                        jobApplicationId={jobApplication.id}
                                    />
                                </>
                            )}

                            {/* At 'INTERVIEWD' state */}
                            {jobApplication.status === 'Interviewed' && (
                                <>
                                    <MarkOfferedButton
                                        jobApplicationId={jobApplication.id}
                                    />
                                    <RejectButton
                                        jobApplicationId={jobApplication.id}
                                    />
                                    <PutOnHoldButton
                                        jobApplicationId={jobApplication.id}
                                    />
                                </>
                            )}

                            {/* At 'OFFERED' state */}
                            {jobApplication.status === 'Offered' && (
                                <>
                                    <MarkHiredButton
                                        jobApplicationId={jobApplication.id}
                                    />
                                    <RejectButton
                                        jobApplicationId={jobApplication.id}
                                    />
                                </>
                            )}

                            {/* At 'HIRED' state */}
                            {jobApplication.status === 'Hired' && (
                                <div className="text-muted-foreground w-full text-center py-2">
                                    No Actions At This Stage
                                </div>
                            )}

                            {/* At 'ON_HOLD' state */}
                            {jobApplication.status === 'OnHold' && (
                                <>
                                    <UnHoldButton
                                        jobApplicationId={jobApplication.id}
                                    />
                                </>
                            )}

                            {/* At 'REJECTED' state */}
                            {jobApplication.status === 'Rejected' && (
                                <>
                                    <RollbackStatusButton
                                        jobApplicationId={jobApplication.id}
                                    />
                                </>
                            )}
                        </div>
                    </div>

                    <div>
                        <div className="flex items-center justify-between">
                            <h3 className="font-semibold text-lg">
                                Review Feedbacks
                            </h3>

                            <AddFeedbackDialog
                                jobApplicationId={jobApplication.id}
                                candidateId={jobApplication.candidateId}
                            />
                        </div>

                        <div className="space-y-3">
                            {jobApplication.jobApplicationFeedbacks.length ===
                                0 && (
                                <p className="text-center py-10 text-muted-foreground">
                                    No feedbacks yet.
                                </p>
                            )}

                            {jobApplication.jobApplicationFeedbacks.map((f) => (
                                <div
                                    key={f.id}
                                    className="border rounded-xl p-3 text-sm space-y-2"
                                >
                                    <div className="flex justify-between">
                                        <span className="font-medium">
                                            Rating: {f.rating} / 10
                                        </span>
                                        <span>
                                            Given by{' '}
                                            <span className="text-muted-foreground">
                                                {f.givenByName}
                                            </span>
                                        </span>
                                    </div>

                                    <p className="text-muted-foreground">
                                        {f.comment}
                                    </p>

                                    <div className="flex flex-wrap">
                                        {f.skillFeedbacks.map((x) => {
                                            return (
                                                <div className="border rounded-sm w-fit text- font-normal m-0.5 py-2 px-2 space-y-0.5">
                                                    <div className="text-center font-semibold">
                                                        {x.skillName}
                                                    </div>
                                                    <div className="font ml-1">
                                                        <span>Rating: </span>
                                                        <span>
                                                            {x.rating}/10
                                                        </span>
                                                    </div>
                                                    <div className="text-sm ml-1">
                                                        <span>
                                                            Assessed Exp.:{' '}
                                                        </span>
                                                        {x.assessedExpYears}{' '}
                                                        Year
                                                    </div>
                                                </div>
                                            );
                                        })}
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>

                    <div>
                        <div className="flex items-center justify-between">
                            <h3 className="font-semibold text-lg">
                                Interview Feedbacks
                            </h3>
                        </div>

                        <div className="space-y-3">
                            {jobApplication.interviewFeedbacks.length === 0 && (
                                <p className="text-center py-10 text-muted-foreground">
                                    No feedbacks yet.
                                </p>
                            )}

                            {jobApplication.interviewFeedbacks.map((f) => (
                                <div
                                    key={f.id}
                                    className="border rounded-xl p-3 text-sm space-y-2"
                                >
                                    <div className="flex justify-between">
                                        <span className="font-medium">
                                            Rating: {f.rating} / 10
                                        </span>
                                        <span>
                                            Given by{' '}
                                            <span className="text-muted-foreground">
                                                {f.givenByName}
                                            </span>
                                        </span>
                                    </div>

                                    <p className="text-muted-foreground">
                                        {f.comment}
                                    </p>

                                    <div className="flex flex-wrap">
                                        {f.skillFeedbacks.map((x) => {
                                            return (
                                                <div className="border rounded-sm w-fit text- font-normal m-0.5 py-2 px-2 space-y-0.5">
                                                    <div className="text-center font-semibold">
                                                        {x.skillName}
                                                    </div>
                                                    <div className="font ml-1">
                                                        <span>Rating: </span>
                                                        <span>
                                                            {x.rating}/10
                                                        </span>
                                                    </div>
                                                    <div className="text-sm ml-1">
                                                        <span>
                                                            Assessed Exp.:{' '}
                                                        </span>
                                                        {x.assessedExpYears}{' '}
                                                        Year
                                                    </div>
                                                </div>
                                            );
                                        })}
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </div>

            <div className="px-10 pt-8 space-y-3">
                <h2 className="font-semibold border-b pb-2">Status History</h2>

                {jobApplication.statusMoveHistories.length === 0 && (
                    <p className="text-center py-10 text-muted-foreground">
                        No status changes yet.
                    </p>
                )}

                <div className="flex flex-wrap gap-1 space-y-4">
                    {jobApplication.statusMoveHistories
                        .sort(
                            (x, y) =>
                                +new Date(x.movedAt) - +new Date(y.movedAt)
                        )
                        .map((h, index) => (
                            <>
                                <div
                                    key={h.id}
                                    className="border w-fit rounded-2xl p-3 text-sm text-center"
                                >
                                    <div className="font-medium text-center mb-2">
                                        {h.statusMovedTo}
                                    </div>

                                    <div className="text-muted-foreground">
                                        At{' '}
                                        {new Date(h.movedAt).toLocaleString()}
                                    </div>

                                    <div className="text-muted-foreground">
                                        by {h.movedByName}
                                    </div>

                                    {h.comment && <p>{h.comment}</p>}
                                </div>

                                {jobApplication.statusMoveHistories.length >
                                    1 &&
                                    index !==
                                        jobApplication.statusMoveHistories
                                            .length -
                                            1 && (
                                        <div className="flex items-center">
                                            <MoveRight className="text-muted-foreground" />
                                        </div>
                                    )}
                            </>
                        ))}
                </div>
            </div>
        </div>
    );
};
