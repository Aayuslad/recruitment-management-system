import { useCreateJobApplications } from '@/api/job-application-api';
import { useGetJobOpenings } from '@/api/job-opening-api';
import { Button } from '@/components/ui/button';
import {
    Dialog,
    DialogClose,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from '@/components/ui/dialog';
import { Spinner } from '@/components/ui/spinner';
import type { CreateJobApplicationsCommandCorrected } from '@/types/job-application-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import z from 'zod';
import { JobOpeningSelector } from './job-opening-selector';
import { useAccessChecker } from '@/hooks/use-has-access';
type Props = {
    selectedCandidates: { id: string }[];
    visibleTo: string[];
};

const createJobApplicationsFormSchema = z.object({
    applications: z.array(
        z.object({
            candidateId: z.string(),
            jobOpeningId: z.string(),
        })
    ),
}) satisfies z.ZodType<CreateJobApplicationsCommandCorrected>;

export function ApplyToJobOpeningDialog({
    selectedCandidates,
    visibleTo,
}: Props) {
    const [open, setOpen] = useState(false);
    const createJobApplications = useCreateJobApplications();
    const { data: jobOpenings, isLoading, isError } = useGetJobOpenings();
    const [selectedJobOpeningId, setSelectedJobOpeningId] = useState<string>();
    const canAccess = useAccessChecker();

    const form = useForm<z.infer<typeof createJobApplicationsFormSchema>>({
        resolver: zodResolver(createJobApplicationsFormSchema),
    });

    useEffect(() => {
        if (!selectedJobOpeningId) return;
        if (!selectedCandidates) return;

        form.setValue(
            'applications',
            selectedCandidates.map((candidate) => ({
                candidateId: candidate.id,
                jobOpeningId: selectedJobOpeningId,
            }))
        );
    }, [selectedCandidates, selectedJobOpeningId]);

    const onSubmit = async (data: CreateJobApplicationsCommandCorrected) => {
        createJobApplications.mutate(data, {
            onSuccess: () => {
                form.reset();
                setOpen(false);
            },
        });
    };

    const onInvalid = (errors: typeof form.formState.errors) => {
        const messages = Object.values(errors).map((err) => err.message);
        messages.reverse().forEach((msg) => toast.error(msg));
    };

    if (!canAccess(visibleTo)) return null;

    if (!jobOpenings && isLoading)
        return (
            <div className="h-[50vh] flex justify-center items-center">
                <Spinner className="size-8" />
            </div>
        );
    if (isError) return <div>Error Loading Job Openings</div>;

    return (
        <Dialog open={open} onOpenChange={setOpen}>
            <DialogTrigger asChild>
                <Button
                    variant="outline"
                    disabled={selectedCandidates.length === 0}
                >
                    Apply to Job Opening
                </Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <form
                    onSubmit={form.handleSubmit(onSubmit, onInvalid)}
                    className="space-y-8"
                >
                    <DialogHeader>
                        <DialogTitle>Apply to Job Opening</DialogTitle>
                        <DialogDescription>
                            Select the job opening and click submit
                        </DialogDescription>
                    </DialogHeader>
                    <div className="grid gap-4">
                        <div className="flex flex-col gap-1">
                            <label>Job Opening</label>
                            <JobOpeningSelector
                                setSelectedJobOpeningId={
                                    setSelectedJobOpeningId
                                }
                            />
                        </div>
                    </div>
                    <DialogFooter>
                        <DialogClose asChild>
                            <Button variant="outline">Cancel</Button>
                        </DialogClose>
                        <Button type="submit">Apply</Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
