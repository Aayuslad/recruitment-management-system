import { useGetJobOpening } from '@/api/job-opening-api';
import { DeleteJobOpeningDialog } from '@/components/jobOpenings/delete-job-opening-dialog';
import { EditJobOpeningSheet } from '@/components/jobOpenings/edit-job-opening-sheet';
import { JobOpeningApplicationsTable } from '@/components/jobOpenings/job-opening-applications-table';
import {
    Accordion,
    AccordionContent,
    AccordionItem,
    AccordionTrigger,
} from '@/components/ui/accordion';
import { SIDEBAR_WIDTH } from '@/components/ui/sidebar';
import { SkillPill } from '@/components/ui/skill-pill';
import { Spinner } from '@/components/ui/spinner';
import {
    Tooltip,
    TooltipContent,
    TooltipTrigger,
} from '@/components/ui/tooltip';
import { useAppStore } from '@/store';
import { INTERVIEW_PARTICIPANT_ROLE } from '@/types/enums';
import { interviewParticipantRoleFormatConverter } from '@/util/interview-participant-role-format-converter';
import { durationFormatConverter } from '@/util/interview-round-duration-format-converter';
import { ExternalLink, Info } from 'lucide-react';
import { useNavigate, useParams } from 'react-router';
import { useShallow } from 'zustand/react/shallow';

export const JobOpeningDetailPage = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { sidebarState } = useAppStore(
        useShallow((s) => ({
            sidebarState: s.sidebarState,
        }))
    );

    const { data, isLoading, isError } = useGetJobOpening(id);

    if (!data || isLoading)
        return (
            <div className="w-full h-[80vh] flex items-center justify-center">
                <Spinner className="size-8" />
            </div>
        );

    if (isError)
        return (
            <div className="w-full h-[80vh] flex items-center justify-center">
                Error fetching job opening
            </div>
        );

    return (
        <div className="h-full mb-10">
            <div className="h-[100px] flex items-center px-10 border-b ">
                <div className="space-y-1">
                    <h1 className="text-2xl font-bold">Job Opening</h1>
                    <div className="space-x-3 font-semibold">
                        <span>{data.designationName}</span>
                        <span>({data.jobLocation})</span>
                    </div>
                </div>
                <div className="ml-auto mb-4 space-x-2">
                    <EditJobOpeningSheet
                        jobOpeningId={data.id}
                        visibleTo={['Admin', 'Recruiter']}
                    />
                    <DeleteJobOpeningDialog
                        jobOpeningId={data.id}
                        visibleTo={['Admin']}
                    />
                </div>
            </div>
            <div
                className="h-auto flex mx-auto justify-center transition-width duration-200 ease-in-out"
                style={{
                    width: `calc(100vw - ${SIDEBAR_WIDTH} - ${sidebarState === 'opened' ? '80px' : '0px'})`,
                }}
            >
                <div className="flex-[40%] px-5 mt-8 space-y-7">
                    <div className="space-y-4 mt-5">
                        <div className="space-y-2">
                            <div className="grid grid-cols-[130px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Job Opening Title
                                </span>
                                <span className="text-sm">{data.title}</span>
                            </div>

                            {/* Description */}
                            {data.description && (
                                <div className="grid grid-cols-[130px_1fr] items-start gap-2">
                                    <span className="text-sm text-muted-foreground">
                                        Description
                                    </span>
                                    <span className="text-sm">
                                        {data.description}
                                    </span>
                                </div>
                            )}

                            <div className="grid grid-cols-[130px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Creation
                                </span>
                                <span className="text-sm">
                                    By{' '}
                                    <span className="underline">
                                        {data.createdByUserName}
                                    </span>{' '}
                                    on{' '}
                                    <span className="font-">
                                        {new Date(
                                            data.createdAt
                                        ).toLocaleDateString()}
                                    </span>
                                </span>
                            </div>
                        </div>
                    </div>

                    <div className="space-y-4">
                        {/* <h3 className="font-semibold text-lg">
                            Position Batch Details
                        </h3> */}

                        <div className="space-y-2">
                            {/* ID Row */}
                            <div className="grid grid-cols-[130px_1fr_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Position Batch
                                </span>
                                <div className="text-sm ">
                                    <a
                                        onClick={() =>
                                            window.open(
                                                `/positions/batch/${data.positionBatchId}`,
                                                '_blank',
                                                'noopener,noreferrer'
                                            )
                                        }
                                        target="_blank"
                                        rel="noopener noreferrer"
                                        className="inline-flex items-center gap-1 text-sm underline hover:cursor-pointer"
                                    >
                                        <span className="text-sm">
                                            View Position Batch
                                        </span>
                                        <ExternalLink className="w-4 h-4" />
                                    </a>
                                </div>
                            </div>

                            {/* Designation */}
                            <div className="grid grid-cols-[130px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Designation
                                </span>
                                <span
                                    className="w-fit hover:cursor-pointer hover:underline"
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        navigate('/configuration/designations');
                                    }}
                                >
                                    {data.designationName}
                                </span>
                            </div>

                            {/* Location */}
                            <div className="grid grid-cols-[130px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Location
                                </span>
                                <span className="text-sm">
                                    {data.jobLocation}
                                </span>
                            </div>

                            {/* CTC */}
                            <div className="grid grid-cols-[130px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    CTC Range
                                </span>
                                <span className="text-sm">
                                    {data.minCTC} – {data.maxCTC} LPA
                                </span>
                            </div>

                            {/* CTC */}
                            <div className="grid grid-cols-[130px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Positions Closed
                                </span>
                                <span className="text-sm">
                                    {data.closedPositionsCount} /{' '}
                                    {data.positionsCount}
                                </span>
                            </div>
                        </div>
                    </div>

                    <div className="space-y-2">
                        <h3 className="font-semibold text-lg">
                            Skills Criteria
                        </h3>
                        <div className="ml-1">
                            {data?.skills
                                .filter((x) => x.skillType === 'Required')
                                .map((x) => {
                                    return (
                                        <SkillPill
                                            id={x.skillId}
                                            name={x.skillName}
                                            type={x.skillType}
                                        />
                                    );
                                })}
                            {data?.skills
                                .filter((x) => x.skillType === 'Preferred')
                                .map((x) => {
                                    return (
                                        <SkillPill
                                            id={x.skillId}
                                            name={x.skillName}
                                            type={x.skillType}
                                        />
                                    );
                                })}
                        </div>
                    </div>
                </div>

                <div className="flex-[60%] px-5 mt-8">
                    <JobOpeningApplicationsTable jobOpeningId={data.id} />
                </div>
            </div>

            <div className="h-auto mt-10 py-10 mx-auto transition-width duration-200 ease-in-out w-[1100px]">
                <Accordion type="single" collapsible className="w-full">
                    <AccordionItem value="item-1" className="mr-19">
                        <AccordionTrigger className="border-b rounded-none px-2">
                            <h3 className="font-semibold text-lg flex items-center gap-1">
                                <span>Interview Templates</span>
                                <span>
                                    <Tooltip>
                                        <TooltipTrigger asChild>
                                            <Info className="w-4 h-4" />
                                        </TooltipTrigger>
                                        <TooltipContent>
                                            <p className="text-wrap max-w-[200px] font-semibold">
                                                These templates will be used to
                                                create automated interview
                                                rounds when a job application is
                                                shortlisted for interview
                                            </p>
                                        </TooltipContent>
                                    </Tooltip>
                                </span>
                            </h3>
                        </AccordionTrigger>
                        <AccordionContent className="flex flex-col gap-4 text-balance">
                            <div className="flex">
                                <div className="flex-[50%] flex justify-center px-5 pt-8 space-y-7">
                                    <div className="space-y-2">
                                        <h3 className="font-semibold text-lg">
                                            Interview Rounds
                                        </h3>
                                        <div className="ml-1 space-y-2.5">
                                            {data.interviewRounds.length ===
                                                0 && (
                                                <div className="text-muted-foreground w-full py-3">
                                                    No Interview Rounds Found
                                                </div>
                                            )}

                                            {data.interviewRounds
                                                .sort(
                                                    (a, b) =>
                                                        a.roundNumber -
                                                        b.roundNumber
                                                )
                                                .map((round, key) => {
                                                    return (
                                                        <div
                                                            className="flex-1 px-1"
                                                            key={key}
                                                        >
                                                            <h4 className="font-semibold space-x-2.5">
                                                                <span>
                                                                    Round{' '}
                                                                    {
                                                                        round.roundNumber
                                                                    }
                                                                </span>
                                                                <span>-</span>
                                                                <span>
                                                                    {round.type}
                                                                </span>
                                                                <span>-</span>
                                                                <span>
                                                                    {durationFormatConverter(
                                                                        round.durationInMinutes
                                                                    )}
                                                                </span>
                                                            </h4>
                                                            <div className="grid grid-cols-[50px_1fr] items-start gap-2">
                                                                <span className="text-sm text-muted-foreground">
                                                                    Panel
                                                                </span>
                                                                <div className="text-sm">
                                                                    {round.requirements.map(
                                                                        (
                                                                            requirement,
                                                                            index
                                                                        ) => (
                                                                            <span>
                                                                                {
                                                                                    requirement.requirementCount
                                                                                }{' '}
                                                                                {interviewParticipantRoleFormatConverter(
                                                                                    requirement.role,
                                                                                    requirement.requirementCount >
                                                                                        1
                                                                                )}
                                                                                {index ===
                                                                                round
                                                                                    .requirements
                                                                                    .length -
                                                                                    1
                                                                                    ? ''
                                                                                    : ', '}
                                                                            </span>
                                                                        )
                                                                    )}
                                                                </div>
                                                            </div>
                                                        </div>
                                                    );
                                                })}
                                        </div>
                                    </div>
                                </div>
                                <div className="flex-[50%] flex justify px-5 pt-8 space-y-7">
                                    <div className="space-y-2">
                                        <h3 className="font-semibold text-lg flex items-center gap-2">
                                            <span>Participant Pool</span>
                                            <span>
                                                <Tooltip>
                                                    <TooltipTrigger asChild>
                                                        <Info className="w-4 h-4" />
                                                    </TooltipTrigger>
                                                    <TooltipContent>
                                                        <p className="text-wrap max-w-[200px] font-semibold">
                                                            These participants
                                                            will be assigned
                                                            randomly to the
                                                            interviews defined
                                                            above, based on
                                                            their role and panel
                                                            requirements.
                                                        </p>
                                                    </TooltipContent>
                                                </Tooltip>
                                            </span>
                                        </h3>
                                        <div className="ml-1 space-y-1.5">
                                            {data.interviewers.length === 0 && (
                                                <div className="text-muted-foreground w-full py-3">
                                                    No Participants Found
                                                </div>
                                            )}

                                            {data.interviewers.filter(
                                                (x) =>
                                                    x.role ===
                                                    INTERVIEW_PARTICIPANT_ROLE.TECHNICAL_INTERVIEWER
                                            ).length > 0 && (
                                                <div>
                                                    <h3 className="font-semibold">
                                                        Technical Interviewers
                                                    </h3>
                                                    <div>
                                                        {data.interviewers
                                                            .filter(
                                                                (x) =>
                                                                    x.role ===
                                                                    INTERVIEW_PARTICIPANT_ROLE.TECHNICAL_INTERVIEWER
                                                            )
                                                            .map(
                                                                (
                                                                    interviewer,
                                                                    key
                                                                ) => (
                                                                    <div
                                                                        className="flex-1 px-1"
                                                                        key={
                                                                            key
                                                                        }
                                                                    >
                                                                        <ul className="ml-5 list-disc">
                                                                            <li className=" space-x-2.5">
                                                                                <span>
                                                                                    {
                                                                                        interviewer.userName
                                                                                    }
                                                                                </span>
                                                                                <span>
                                                                                    (
                                                                                    {
                                                                                        interviewer.email
                                                                                    }

                                                                                    )
                                                                                </span>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                )
                                                            )}
                                                    </div>
                                                </div>
                                            )}

                                            {data.interviewers.filter(
                                                (x) =>
                                                    x.role ===
                                                    INTERVIEW_PARTICIPANT_ROLE.HR_INTERVIEWER
                                            ).length > 0 && (
                                                <div>
                                                    <h3 className="font-semibold">
                                                        HR Interviewers
                                                    </h3>
                                                    <div>
                                                        {data.interviewers
                                                            .filter(
                                                                (x) =>
                                                                    x.role ===
                                                                    INTERVIEW_PARTICIPANT_ROLE.HR_INTERVIEWER
                                                            )
                                                            .map(
                                                                (
                                                                    interviewer,
                                                                    key
                                                                ) => (
                                                                    <div
                                                                        className="flex-1 px-1"
                                                                        key={
                                                                            key
                                                                        }
                                                                    >
                                                                        <ul className="ml-5 list-disc">
                                                                            <li className=" space-x-2.5">
                                                                                <span>
                                                                                    {
                                                                                        interviewer.userName
                                                                                    }
                                                                                </span>
                                                                                <span>
                                                                                    (
                                                                                    {
                                                                                        interviewer.email
                                                                                    }

                                                                                    )
                                                                                </span>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                )
                                                            )}
                                                    </div>
                                                </div>
                                            )}

                                            {data.interviewers.filter(
                                                (x) =>
                                                    x.role ===
                                                    INTERVIEW_PARTICIPANT_ROLE.OBSERVER
                                            ).length > 0 && (
                                                <div>
                                                    <h3 className="font-semibold">
                                                        Observers
                                                    </h3>
                                                    <div>
                                                        {data.interviewers
                                                            .filter(
                                                                (x) =>
                                                                    x.role ===
                                                                    INTERVIEW_PARTICIPANT_ROLE.TECHNICAL_INTERVIEWER
                                                            )
                                                            .map(
                                                                (
                                                                    interviewer,
                                                                    key
                                                                ) => (
                                                                    <div
                                                                        className="flex-1 px-1"
                                                                        key={
                                                                            key
                                                                        }
                                                                    >
                                                                        <ul className="ml-5 list-disc">
                                                                            <li className=" space-x-2.5">
                                                                                <span>
                                                                                    {
                                                                                        interviewer.userName
                                                                                    }
                                                                                </span>
                                                                                <span>
                                                                                    (
                                                                                    {
                                                                                        interviewer.email
                                                                                    }

                                                                                    )
                                                                                </span>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                )
                                                            )}
                                                    </div>
                                                </div>
                                            )}

                                            {data.interviewers.filter(
                                                (x) =>
                                                    x.role ===
                                                    INTERVIEW_PARTICIPANT_ROLE.NOTE_TAKER
                                            ).length > 0 && (
                                                <div>
                                                    <h3 className="font-semibold">
                                                        Note Takers
                                                    </h3>
                                                    <div>
                                                        {data.interviewers
                                                            .filter(
                                                                (x) =>
                                                                    x.role ===
                                                                    INTERVIEW_PARTICIPANT_ROLE.TECHNICAL_INTERVIEWER
                                                            )
                                                            .map(
                                                                (
                                                                    interviewer,
                                                                    key
                                                                ) => (
                                                                    <div
                                                                        className="flex-1 px-1"
                                                                        key={
                                                                            key
                                                                        }
                                                                    >
                                                                        <ul className="ml-5 list-disc">
                                                                            <li className=" space-x-2.5">
                                                                                <span>
                                                                                    {
                                                                                        interviewer.userName
                                                                                    }
                                                                                </span>
                                                                                <span>
                                                                                    (
                                                                                    {
                                                                                        interviewer.email
                                                                                    }

                                                                                    )
                                                                                </span>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                )
                                                            )}
                                                    </div>
                                                </div>
                                            )}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </AccordionContent>
                    </AccordionItem>
                </Accordion>
            </div>
        </div>
    );
};
