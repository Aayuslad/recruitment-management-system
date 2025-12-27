import { useGetCandidate } from '@/api/candidate-api';
import { useGetJobApplication } from '@/api/job-application-api';
import { useGetJobOpening } from '@/api/job-opening-api';
import { AddFeedbackDialog } from '@/components/jobApplications/internal/add-feedback-dialog';
import { RejectButton } from '@/components/jobApplications/stateActionButtons/reject';
import { ShortlistButton } from '@/components/jobApplications/stateActionButtons/shortlist';
import { SIDEBAR_WIDTH } from '@/components/ui/sidebar';
import { SkillPill } from '@/components/ui/skill-pill';
import { Spinner } from '@/components/ui/spinner';
import { useAppStore } from '@/store';
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

export const ScreeningApplicationDetailsPage = () => {
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

    if (isJobApplicationLoading || isCandidateLoading || isJobOpeningLoading)
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
            <div className="h-30 flex items-center px-10 border-b">
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

                <div className="ml-auto flex items-center gap-3">
                    {jobApplication?.status === 'Applied' && (
                        <>
                            <ShortlistButton
                                jobApplicationId={jobApplication.id}
                                visibleTo={['Reviewer']}
                            />
                            <RejectButton
                                jobApplicationId={jobApplication.id}
                                visibleTo={['Reviewer']}
                            />
                        </>
                    )}
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
                                        <span className="text-sm">
                                            View Resume
                                        </span>
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
                    </div>

                    <div className="space-y-4">
                        <h3 className="font-semibold text-lg">
                            Skill Criteria Evaluation
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
                                                minExperienceYears={
                                                    skill.minExperienceYears as number
                                                }
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
                                                minExperienceYears={
                                                    skill.minExperienceYears as number
                                                }
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
                                                minExperienceYears={null}
                                            />
                                        );
                                    })}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="flex-[50%] px-5 pt-8 space-y-6 ">
                    <div className="flex items-center justify-between">
                        <h3 className="font-semibold text-lg">
                            Review Feedbacks{' '}
                            <span className="text-muted-foreground">
                                {jobApplication?.avgRating
                                    ? `(${jobApplication?.avgRating} / 10 Average)`
                                    : ''}
                            </span>
                        </h3>

                        <AddFeedbackDialog
                            jobApplicationId={jobApplication?.id}
                            candidateId={jobApplication?.candidateId}
                        />
                    </div>

                    <div className="space-y-3 max-h-[600px] overflow-y-auto">
                        {jobApplication?.jobApplicationFeedbacks.length ===
                            0 && (
                            <p className="text-center py-10 text-muted-foreground">
                                No feedbacks yet.
                            </p>
                        )}

                        {jobApplication?.jobApplicationFeedbacks.map((f) => (
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
                                                            {x.assessedExpYears}{' '}
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
    );
};
