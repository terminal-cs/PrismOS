if [![ -f "C:\Program Files\Intel\HAXM\IntelHaxm.sys" ]]
if [[ -f "/bin/qemu-system-x86_64" ]]
then
	/bin/qemu-system-x86_64 -accel kvm -m 1024 -smp 2 -k en-gb -boot d -d guest_errors -serial stdio -device AC97 -rtc base=localtime -bios ../CoreLib/Disk/OVMF.fd -drive file=fat:rw:../CoreLib/Disk -net nic,model=rtl8139 -net tap,ifname=tap
	
elif
	echo "Please install QEMU in order to debug!(do not modify the path) https://www.qemu.org/download/#linux"
	pause
	exit
fi