import { useGetCandidate } from '@/api/candidate-api';
import { useGetJobApplicationInterviews } from '@/api/interviews-api';
import { useGetJobApplication } from '@/api/job-application-api';
import { useGetJobOpening } from '@/api/job-opening-api';
import { AddFeedbackDialog } from '@/components/jobApplications/internal/add-feedback-dialog';
import { ApplicationProgress } from '@/components/jobApplications/job-application-progress';
import { MarkHiredButton } from '@/components/jobApplications/stateActionButtons/mark-hired';
import { MarkOfferedButton } from '@/components/jobApplications/stateActionButtons/mark-offered';
import { PutOnHoldButton } from '@/components/jobApplications/stateActionButtons/put-on-hold';
import { RejectButton } from '@/components/jobApplications/stateActionButtons/reject';
import { RollbackStatusButton } from '@/components/jobApplications/stateActionButtons/rollback-status';
import { UnHoldButton } from '@/components/jobApplications/stateActionButtons/unhold';
import {
    Accordion,
    AccordionContent,
    AccordionItem,
    AccordionTrigger,
} from '@/components/ui/accordion';
import { SIDEBAR_WIDTH } from '@/components/ui/sidebar';
import { SkillPill } from '@/components/ui/skill-pill';
import { Spinner } from '@/components/ui/spinner';
import { useAppStore } from '@/store';
import { INTERVIEW_STATUS } from '@/types/enums';
import { durationFormatConverter } from '@/util/interview-round-duration-format-converter';
import { timeAgo } from '@/util/time-ago';
import { ExternalLink } from 'lucide-react';
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
    const {
        data: interviews,
        isLoading: isInterviewsLoading,
        isError: isInterviewsError,
    } = useGetJobApplicationInterviews(id);

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
        isJobApplicationLoading ||
        isCandidateLoading ||
        isJobOpeningLoading ||
        isInterviewsLoading
    )
        return (
            <div className="w-full h-[80vh] flex items-center justify-center">
                <Spinner className="size-8" />
            </div>
        );

    if (
        isCandidateError ||
        isJobApplicationError ||
        isJobOpeningError ||
        isInterviewsError
    )
        return (
            <div className="w-full h-[80vh] flex items-center justify-center">
                Error fetching data
            </div>
        );

    return (
        <div className="h-full flex flex-col mb-10">
            <div className="h-45 flex items-center justify-between px-10 border-b">
                <div className="space-y-1">
                    <div className="flex space-x-2">
                        <h1 className="text-2xl font-bold">Job Application</h1>
                    </div>

                    <div className="flex items-center gap-3 text-sm text-muted-foreground">
                        <div>{`${candidate?.firstName} ${candidate?.middleName} ${candidate?.lastName}`}</div>
                        <span>•</span>
                        <span>{jobApplication?.designation}</span>
                        <span>•</span>
                        <span>
                            Applied {timeAgo(jobApplication?.appliedAt ?? '')}
                        </span>
                    </div>
                </div>

                <div className="min-w-0 w-[400px] whitespace-nowrap mr-10">
                    <ApplicationProgress status={jobApplication?.status} />
                </div>
            </div>

            <div
                className="h-full flex mx-auto justify-center transition-width duration-200 ease-in-out"
                style={{
                    width: `calc(100vw - ${SIDEBAR_WIDTH} - ${
                        sidebarState === 'opened' ? '80px' : '0px'
                    })`,
                }}
            >
                <div className="flex-[50%] px-5 pt-8 space-y-8">
                    <div className="space-y-4 mt-4">
                        <div className="flex flex-col gap-2">
                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Candidate
                                </span>
                                <a
                                    onClick={() =>
                                        window.open(
                                            `/candidates/candidate/${candidate?.id}`,
                                            '_blank',
                                            'noopener,noreferrer'
                                        )
                                    }
                                    target="_blank"
                                    rel="noopener noreferrer"
                                    className="inline-flex w-fit items-center gap-1 text-sm underline hover:cursor-pointer"
                                >
                                    <span className="text-sm">{`${candidate?.firstName} ${candidate?.middleName} ${candidate?.lastName}`}</span>
                                    <ExternalLink className="w-4 h-4" />
                                </a>
                            </div>

                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Candidate Resume
                                </span>
                                <a
                                    href={candidate?.resumeUrl}
                                    target="_blank"
                                    rel="noopener noreferrer"
                                    className="inline-flex w-fit items-center gap-1 text-sm underline hover:cursor-pointer"
                                >
                                    <span className="text-sm">View Resume</span>
                                    <ExternalLink className="w-4 h-4" />
                                </a>
                            </div>
                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Job Opening
                                </span>
                                <a
                                    onClick={() =>
                                        window.open(
                                            `/job-openings/opening/${jobOpening?.id}`,
                                            '_blank',
                                            'noopener,noreferrer'
                                        )
                                    }
                                    target="_blank"
                                    rel="noopener noreferrer"
                                    className="inline-flex w-fit items-center gap-1 text-sm underline hover:cursor-pointer"
                                >
                                    <span className="text-sm">
                                        View Job Opening
                                    </span>
                                    <ExternalLink className="w-4 h-4" />
                                </a>
                            </div>
                        </div>
                    </div>

                    <div className="space-y-4">
                        <h3 className="font-semibold text-lg">
                            Skill Evaluation
                        </h3>
                        <div className="flex flex-col gap-2.5">
                            <div className="space-y-1.5">
                                <h4>Matched Skills</h4>

                                <div>
                                    {matchedSkills.length === 0 && (
                                        <div className="w-[500px] text-sm pb-1 flex items-center justify-center text-muted-foreground">
                                            No Matched Skills
                                        </div>
                                    )}

                                    {[
                                        ...matchedSkills.filter(
                                            (x) => x.skillType === 'Required'
                                        ),
                                        ...matchedSkills.filter(
                                            (x) => x.skillType === 'Preferred'
                                        ),
                                    ].map((skill) => {
                                        return (
                                            <SkillPill
                                                id={skill.skillId}
                                                name={skill.skillName}
                                                type={skill.skillType as string}
                                            />
                                        );
                                    })}
                                </div>
                            </div>

                            <div className="space-y-1.5">
                                <h4>Missing Skills</h4>

                                <div>
                                    {missingSkills.length === 0 && (
                                        <div className="w-[500px] text-sm pb-1 flex items-center justify-center text-muted-foreground">
                                            No Missing Skills
                                        </div>
                                    )}

                                    {[
                                        ...missingSkills.filter(
                                            (x) => x.skillType === 'Required'
                                        ),
                                        ...missingSkills.filter(
                                            (x) => x.skillType === 'Preferred'
                                        ),
                                    ].map((skill) => {
                                        return (
                                            <SkillPill
                                                id={skill.skillId}
                                                name={skill.skillName}
                                                type={skill.skillType as string}
                                            />
                                        );
                                    })}
                                </div>
                            </div>

                            <div className="space-y-1.5">
                                <h4>Additional Skills</h4>

                                <div>
                                    {additionalSkills.length === 0 && (
                                        <div className="w-[500px] text-sm pb-1 flex items-center justify-center text-muted-foreground">
                                            No Additional Skills
                                        </div>
                                    )}

                                    {additionalSkills.map((skill) => {
                                        return (
                                            <SkillPill
                                                id={skill.skillId}
                                                name={skill.skillName}
                                                type={null}
                                            />
                                        );
                                    })}
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="space-y-4">
                        <h3 className="font-semibold text-lg">Interviews</h3>

                        {interviews?.length === 0 && (
                            <div className="w-[500px] py-3 flex items-center justify-center text-muted-foreground">
                                No Interviews Scheduled
                            </div>
                        )}

                        <Accordion
                            type="single"
                            collapsible
                            className="w-[550px] -mt-2"
                        >
                            {interviews?.map((x) => {
                                return (
                                    <AccordionItem value={x.id}>
                                        <AccordionTrigger className="hover:no-underline hover:cursor-pointer px-1 border-b rounded-none">
                                            <div className="flex gap-2.5 w-full">
                                                <span>
                                                    Round {x.roundNumber}
                                                </span>
                                                <span>•</span>
                                                <span>{x.interviewType}</span>
                                                <span>•</span>
                                                <span>
                                                    {durationFormatConverter(
                                                        x.durationInMinutes
                                                    )}
                                                </span>
                                                <div className="ml-auto">
                                                    {x.status ===
                                                        INTERVIEW_STATUS.NOT_SCHEDULED && (
                                                        <span>
                                                            Not Scheduled
                                                        </span>
                                                    )}

                                                    {x.status ===
                                                        INTERVIEW_STATUS.SCHEDULED && (
                                                        <span>
                                                            At{' '}
                                                            {new Date(
                                                                x.scheduledAt ??
                                                                    ''
                                                            ).toLocaleString()}
                                                        </span>
                                                    )}

                                                    {x.status ===
                                                        INTERVIEW_STATUS.COMPLETED && (
                                                        <span>Completed</span>
                                                    )}
                                                </div>
                                            </div>
                                        </AccordionTrigger>
                                        <AccordionContent className="flex flex-col gap-4 text-balance pt-2 pb-4 px-3">
                                            <h4>Participants:</h4>

                                            <div className="space-y-1.5">
                                                {x.participants.map((x) => {
                                                    return (
                                                        <div
                                                            key={x.id}
                                                            className="flex justify-between px-2"
                                                        >
                                                            <span>
                                                                {
                                                                    x.participantUserName
                                                                }
                                                            </span>
                                                            <span>
                                                                {x.role}
                                                            </span>
                                                        </div>
                                                    );
                                                })}
                                                {x.participants.length ===
                                                    0 && (
                                                    <div className="text-muted-foreground w-full text-center">
                                                        No participants
                                                    </div>
                                                )}
                                            </div>
                                        </AccordionContent>
                                    </AccordionItem>
                                );
                            })}
                        </Accordion>
                    </div>
                </div>

                <div className="flex-[50%] px-5 pt-8 space-y-6 ">
                    <div className="space-y-1">
                        <h3 className="font-semibold text-lg">
                            Status Actions
                        </h3>

                        <div className="rounded-xl flex gap-3 justify-center">
                            {/* At 'APPLIED' state */}
                            {jobApplication?.status === 'Applied' && (
                                <>
                                    <RejectButton
                                        jobApplicationId={jobApplication.id}
                                        visibleTo={['Admin', 'Recruiter']}
                                    />
                                    <PutOnHoldButton
                                        jobApplicationId={jobApplication.id}
                                        visibleTo={['Admin', 'Recruiter']}
                                    />
                                </>
                            )}

                            {/* At 'SHORTLISTED' state */}
                            {jobApplication?.status === 'Shortlisted' && (
                                <>
                                    {/* <MarkInterviewedButton
                                        jobApplicationId={jobApplication.id}
                                        visibleTo={['Admin', 'Interviewer']}
                                    /> */}
                                    <RejectButton
                                        jobApplicationId={jobApplication.id}
                                        visibleTo={['Admin', 'Interviewer']}
                                    />
                                    <PutOnHoldButton
                                        jobApplicationId={jobApplication.id}
                                        visibleTo={['Admin', 'Recruiter']}
                                    />
                                </>
                            )}

                            {/* At 'INTERVIEWED' state */}
                            {jobApplication?.status === 'Interviewed' && (
                                <>
                                    <MarkOfferedButton
                                        jobApplicationId={jobApplication.id}
                                        visibleTo={['Admin', 'HR']}
                                    />
                                    <RejectButton
                                        jobApplicationId={jobApplication.id}
                                        visibleTo={['Admin', 'HR']}
                                    />
                                    <PutOnHoldButton
                                        jobApplicationId={jobApplication.id}
                                        visibleTo={['Admin', 'Recruiter']}
                                    />
                                </>
                            )}

                            {/* At 'OFFERED' state */}
                            {jobApplication?.status === 'Offered' && (
                                <>
                                    <MarkHiredButton
                                        jobApplicationId={jobApplication.id}
                                        visibleTo={['Admin', 'HR']}
                                    />
                                    <RejectButton
                                        jobApplicationId={jobApplication.id}
                                        visibleTo={['Admin', 'HR']}
                                    />
                                </>
                            )}

                            {/* At 'HIRED' state */}
                            {jobApplication?.status === 'Hired' && (
                                <div className="text-muted-foreground w-full text-center py-2">
                                    No Actions At This Stage
                                </div>
                            )}

                            {/* At 'ON_HOLD' state */}
                            {jobApplication?.status === 'OnHold' && (
                                <>
                                    <UnHoldButton
                                        jobApplicationId={jobApplication.id}
                                        visibleTo={['Admin', 'Recruiter']}
                                    />
                                </>
                            )}

                            {/* At 'REJECTED' state */}
                            {jobApplication?.status === 'Rejected' && (
                                <>
                                    <RollbackStatusButton
                                        jobApplicationId={jobApplication.id}
                                        visibleTo={['Admin', 'Recruiter']}
                                    />
                                </>
                            )}
                        </div>
                    </div>

                    <div className="space-y-4">
                        <div className="flex items-center justify-between">
                            <h3 className="font-semibold text-lg">
                                Feedbacks{' '}
                                <span className="text-muted-foreground">
                                    {jobApplication?.avgRating
                                        ? `(${jobApplication?.avgRating} / 10 Average)`
                                        : ''}
                                </span>
                            </h3>

                            <AddFeedbackDialog
                                jobApplicationId={jobApplication?.id}
                                candidateId={jobApplication?.candidateId}
                                visibleTo={['Admin', 'Recruiter']}
                            />
                        </div>

                        <div className="space-y-3 max-h-[600px] overflow-y-auto">
                            {jobApplication?.jobApplicationFeedbacks.length ===
                                0 &&
                                jobApplication?.interviewFeedbacks.length ===
                                    0 && (
                                    <p className="text-center py-10 text-muted-foreground">
                                        No feedbacks yet.
                                    </p>
                                )}

                            {[
                                ...(jobApplication?.jobApplicationFeedbacks ??
                                    []),
                                ...(jobApplication?.interviewFeedbacks ?? []),
                            ].map((f) => (
                                <div
                                    key={f.id}
                                    className="border rounded-lg p-4 space-y-3"
                                >
                                    <div className="flex items-start justify-between gap-4">
                                        <div className="flex-1 space-y-1">
                                            <div className="flex items-center gap-2">
                                                <div className="text-base font-semibold">
                                                    {f.rating}/10
                                                </div>
                                            </div>
                                        </div>
                                        <div className="text-right text-xs text-muted-foreground">
                                            <div className="text-xs text-muted-foreground space-y-0.5">
                                                <p>
                                                    Given by{' '}
                                                    <span className="font-medium">
                                                        {f.givenByName}
                                                    </span>
                                                </p>
                                                <p>
                                                    While{' '}
                                                    <span className="font-medium">
                                                        {f.stage}
                                                    </span>
                                                </p>
                                            </div>
                                        </div>
                                    </div>

                                    {f.comment && (
                                        <p className="text-sm leading-relaxed">
                                            {f.comment}
                                        </p>
                                    )}

                                    {f.skillFeedbacks.length > 0 && (
                                        <div className="space-y-2 pt-2 border-t">
                                            <p className="text-xs font-semibold text-muted-foreground">
                                                Skills Assessed
                                            </p>
                                            <div className="flex flex-wrap gap-2">
                                                {f.skillFeedbacks.map((x) => (
                                                    <div
                                                        key={x.skillName}
                                                        className="border rounded-md p-2.5 space-y-1 bg-muted/30"
                                                    >
                                                        <p className="font-medium text-sm">
                                                            {x.skillName}
                                                        </p>
                                                        <div className="flex justify-between text-xs text-muted-foreground">
                                                            <span>
                                                                Rating:{' '}
                                                                <span className="font-semibold">
                                                                    {x.rating}
                                                                    /10
                                                                </span>
                                                            </span>
                                                        </div>
                                                        <div className="text-xs text-muted-foreground">
                                                            Assessed Exp.:{' '}
                                                            <span className="font-semibold">
                                                                {
                                                                    x.assessedExpYears
                                                                }{' '}
                                                                Year
                                                            </span>
                                                        </div>
                                                    </div>
                                                ))}
                                            </div>
                                        </div>
                                    )}
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </div>

            <div className="px-10 pt-8 space-y-3">
                <h2 className="font-semibold border-b pb-2">Status History</h2>

                {jobApplication?.statusMoveHistories.length === 0 && (
                    <p className="text-center py-10 text-muted-foreground">
                        No status changes yet.
                    </p>
                )}

                <div className="flex flex-wrap gap-4 py-4">
                    {jobApplication?.statusMoveHistories
                        .sort(
                            (x, y) =>
                                +new Date(x.movedAt) - +new Date(y.movedAt)
                        )
                        .map((h, index) => (
                            <div key={h.id} className="flex items-center gap-4">
                                {/* Timeline dot and line */}
                                <div className="flex items-center">
                                    <div className="w-4 h-4 bg-foreground rounded-full border-4 border-background z-10" />
                                    {index !==
                                        jobApplication.statusMoveHistories
                                            .length -
                                            1 && (
                                        <div className="w-24 h-1 bg-muted mt-0" />
                                    )}
                                </div>

                                {/* Content */}
                                <div>
                                    <div className="font-semibold text-base">
                                        {h.statusMovedTo}
                                    </div>
                                    <div className="text-sm text-muted-foreground">
                                        {new Date(h.movedAt).toLocaleString()}
                                    </div>
                                    <div className="text-sm text-muted-foreground">
                                        by {h.movedByName}
                                    </div>
                                    {h.comment && (
                                        <p className="text-sm mt-1 italic text-muted-foreground">
                                            "{h.comment}"
                                        </p>
                                    )}
                                </div>
                            </div>
                        ))}
                </div>
            </div>
        </div>
    );
};
